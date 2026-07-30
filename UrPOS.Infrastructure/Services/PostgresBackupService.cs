using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.Infrastructure.Services
{
    /// <summary>
    /// Creates and restores AES-256 encrypted PostgreSQL backups (.upbak)
    /// using <c>pg_dump</c> / <c>psql</c> and PBKDF2 key derivation.
    /// </summary>
    public sealed class PostgresBackupService : IBackupService
    {
        private const string FileMagic = "UPBAK";
        private const byte FileVersion = 1;
        private const int SaltSize = 16;
        private const int IvSize = 16;
        private const int KeySize = 32;
        private const int Pbkdf2Iterations = 100_000;

        /// <summary>
        /// SET parameters that newer pg_dump versions emit but older servers reject.
        /// </summary>
        private static readonly (string Name, int MinServerMajor)[] VersionedSetParameters =
        [
            ("transaction_timeout", 17)
        ];

        private readonly AppConfigurations _configs;

        public PostgresBackupService(AppConfigurations configs)
        {
            _configs = configs;
        }

        public async Task<ServiceResult> CreateEncryptedBackupAsync(string destinationFilePath, string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(destinationFilePath) || string.IsNullOrWhiteSpace(encryptionKey))
            {
                return ServiceResult.Failure("مسار الملف أو كلمة المرور غير صالحين.");
            }

            string? tempDumpPath = null;
            try
            {
                var serverMajor = await GetServerMajorVersionAsync();
                if (!TryResolveTool("pg_dump.exe", serverMajor, out var pgDumpPath))
                {
                    return ServiceResult.Failure("لم يتم العثور على pg_dump.exe. ثبّت أدوات PostgreSQL أو أضف مجلد bin إلى PATH.");
                }

                tempDumpPath = Path.Combine(Path.GetTempPath(), $"urpos_dump_{Guid.NewGuid():N}.sql");

                var dumpResult = await RunProcessAsync(
                    pgDumpPath,
                    BuildDumpArguments(tempDumpPath),
                    _configs.DbPassword);

                if (!dumpResult.isSuccess || !File.Exists(tempDumpPath) || new FileInfo(tempDumpPath).Length == 0)
                {
                    return ServiceResult.Failure(
                        string.IsNullOrWhiteSpace(dumpResult.ErrorMessage)
                            ? "فشل إنشاء نسخة SQL عبر pg_dump."
                            : dumpResult.ErrorMessage);
                }

                await SanitizeDumpForServerAsync(tempDumpPath, serverMajor);

                var directory = Path.GetDirectoryName(destinationFilePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await EncryptFileAsync(tempDumpPath, destinationFilePath, encryptionKey);
                return File.Exists(destinationFilePath)
                    ? ServiceResult.Success()
                    : ServiceResult.Failure("تعذّر كتابة ملف النسخة الاحتياطية المشفرة.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PostgresBackupService.CreateEncryptedBackupAsync: {ex}");
                return ServiceResult.Failure($"فشل النسخ الاحتياطي: {ex.Message}");
            }
            finally
            {
                SecureDeleteFile(tempDumpPath);
            }
        }

        public async Task<ServiceResult> RestoreEncryptedBackupAsync(string sourceFilePath, string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(sourceFilePath)
                || string.IsNullOrWhiteSpace(encryptionKey)
                || !File.Exists(sourceFilePath))
            {
                return ServiceResult.Failure("ملف النسخة الاحتياطية أو كلمة المرور غير صالحين.");
            }

            string? tempDumpPath = null;
            try
            {
                var serverMajor = await GetServerMajorVersionAsync();
                if (!TryResolveTool("psql.exe", serverMajor, out var psqlPath))
                {
                    return ServiceResult.Failure("لم يتم العثور على psql.exe. ثبّت أدوات PostgreSQL أو أضف مجلد bin إلى PATH.");
                }

                tempDumpPath = Path.Combine(Path.GetTempPath(), $"urpos_restore_{Guid.NewGuid():N}.sql");

                try
                {
                    await DecryptFileAsync(sourceFilePath, tempDumpPath, encryptionKey);
                }
                catch (CryptographicException)
                {
                    return ServiceResult.Failure("فشل فك التشفير. تأكد من كلمة المرور وصحة ملف .upbak.");
                }

                if (!File.Exists(tempDumpPath) || new FileInfo(tempDumpPath).Length == 0)
                {
                    return ServiceResult.Failure("فشل فك التشفير أو أن الملف الناتج فارغ.");
                }

                // Existing backups may have been created with a newer pg_dump (e.g. 18 vs server 14).
                await SanitizeDumpForServerAsync(tempDumpPath, serverMajor);

                // Drop pooled app connections so DROP/CREATE in the dump can proceed.
                NpgsqlConnection.ClearAllPools();
                await TerminateOtherConnectionsAsync(psqlPath);

                var restoreResult = await RunProcessAsync(
                    psqlPath,
                    BuildRestoreArguments(tempDumpPath),
                    _configs.DbPassword);

                return restoreResult.isSuccess
                    ? ServiceResult.Success()
                    : ServiceResult.Failure(
                        string.IsNullOrWhiteSpace(restoreResult.ErrorMessage)
                            ? "فشل تنفيذ الاستعادة عبر psql."
                            : restoreResult.ErrorMessage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PostgresBackupService.RestoreEncryptedBackupAsync: {ex}");
                return ServiceResult.Failure($"فشلت الاستعادة: {ex.Message}");
            }
            finally
            {
                SecureDeleteFile(tempDumpPath);
                NpgsqlConnection.ClearAllPools();
            }
        }

        private List<string> BuildDumpArguments(string outputPath) =>
        [
            "-h", _configs.DbHost,
            "-p", _configs.DbPort.ToString(),
            "-U", _configs.DbUsername,
            "-d", _configs.DbName,
            "-F", "p",
            "--clean",
            "--if-exists",
            "--no-owner",
            "--no-acl",
            "-f", outputPath
        ];

        private List<string> BuildRestoreArguments(string inputPath) =>
        [
            "-h", _configs.DbHost,
            "-p", _configs.DbPort.ToString(),
            "-U", _configs.DbUsername,
            "-d", _configs.DbName,
            "-v", "ON_ERROR_STOP=1",
            "-f", inputPath
        ];

        private async Task<int> GetServerMajorVersionAsync()
        {
            await using var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();
            await using var command = new NpgsqlCommand("SHOW server_version_num", connection);
            var value = await command.ExecuteScalarAsync();
            if (value is string text && int.TryParse(text, out var versionNum) && versionNum >= 10000)
            {
                return versionNum / 10000;
            }

            return 0;
        }

        private async Task TerminateOtherConnectionsAsync(string psqlPath)
        {
            // Connect to maintenance DB so we can kick sessions off the target database.
            var sql =
                $"SELECT pg_terminate_backend(pid) FROM pg_stat_activity " +
                $"WHERE datname = '{_configs.DbName.Replace("'", "''")}' AND pid <> pg_backend_pid();";

            var args = new List<string>
            {
                "-h", _configs.DbHost,
                "-p", _configs.DbPort.ToString(),
                "-U", _configs.DbUsername,
                "-d", "postgres",
                "-v", "ON_ERROR_STOP=1",
                "-c", sql
            };

            var result = await RunProcessAsync(psqlPath, args, _configs.DbPassword);
            if (!result.isSuccess)
            {
                Debug.WriteLine($"PostgresBackupService.TerminateOtherConnectionsAsync: {result.ErrorMessage}");
            }
        }

        /// <summary>
        /// Removes dump directives that newer pg_dump clients inject but older
        /// servers/psql reject (e.g. SET transaction_timeout, \restrict).
        /// </summary>
        private static async Task SanitizeDumpForServerAsync(string dumpPath, int serverMajor)
        {
            if (!File.Exists(dumpPath))
            {
                return;
            }

            var unsupported = VersionedSetParameters
                .Where(p => serverMajor > 0 && serverMajor < p.MinServerMajor)
                .Select(p => p.Name)
                .ToArray();

            // \restrict / \unrestrict were added in pg_dump 17+; psql 14 rejects them.
            var restrictPattern = new Regex(
                @"^\s*\\(un)?restrict\b",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            Regex? setPattern = unsupported.Length == 0
                ? null
                : new Regex(
                    $@"^\s*SET\s+({string.Join('|', unsupported.Select(Regex.Escape))})\s*=",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            var lines = await File.ReadAllLinesAsync(dumpPath);
            var filtered = lines
                .Where(line =>
                    !restrictPattern.IsMatch(line)
                    && (setPattern is null || !setPattern.IsMatch(line)))
                .ToArray();

            if (filtered.Length != lines.Length)
            {
                await File.WriteAllLinesAsync(dumpPath, filtered);
            }
        }

        private static async Task EncryptFileAsync(string plainPath, string destinationPath, string encryptionKey)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var iv = RandomNumberGenerator.GetBytes(IvSize);
            var key = DeriveKey(encryptionKey, salt);

            await using var destination = new FileStream(
                destinationPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            var magicBytes = Encoding.ASCII.GetBytes(FileMagic);
            await destination.WriteAsync(magicBytes);
            destination.WriteByte(FileVersion);
            await destination.WriteAsync(salt);
            await destination.WriteAsync(iv);

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            await using (var cryptoStream = new CryptoStream(
                               destination,
                               aes.CreateEncryptor(),
                               CryptoStreamMode.Write,
                               leaveOpen: true))
            await using (var source = new FileStream(plainPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await source.CopyToAsync(cryptoStream);
                await cryptoStream.FlushFinalBlockAsync();
            }

            await destination.FlushAsync();
        }

        private static async Task DecryptFileAsync(string encryptedPath, string plainPath, string encryptionKey)
        {
            await using var source = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var magicBuffer = new byte[FileMagic.Length];
            if (await source.ReadAsync(magicBuffer) != magicBuffer.Length
                || Encoding.ASCII.GetString(magicBuffer) != FileMagic)
            {
                throw new CryptographicException("Invalid backup file header.");
            }

            var version = source.ReadByte();
            if (version != FileVersion)
            {
                throw new CryptographicException("Unsupported backup file version.");
            }

            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];
            if (await source.ReadAsync(salt) != SaltSize || await source.ReadAsync(iv) != IvSize)
            {
                throw new CryptographicException("Backup file is truncated or corrupt.");
            }

            var key = DeriveKey(encryptionKey, salt);

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            // Flush/close the destination before any FileInfo length checks.
            await using (var cryptoStream = new CryptoStream(
                               source,
                               aes.CreateDecryptor(),
                               CryptoStreamMode.Read,
                               leaveOpen: true))
            await using (var destination = new FileStream(
                               plainPath,
                               FileMode.Create,
                               FileAccess.Write,
                               FileShare.None))
            {
                await cryptoStream.CopyToAsync(destination);
                await destination.FlushAsync();
            }
        }

        private static byte[] DeriveKey(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Pbkdf2Iterations,
                HashAlgorithmName.SHA256,
                KeySize);
        }

        private static async Task<ServiceResult> RunProcessAsync(
            string fileName,
            IReadOnlyList<string> arguments,
            string password)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            foreach (var argument in arguments)
            {
                startInfo.ArgumentList.Add(argument);
            }

            startInfo.Environment["PGPASSWORD"] = password ?? string.Empty;

            using var process = new Process { StartInfo = startInfo };
            var stdErr = new StringBuilder();
            var stdOut = new StringBuilder();

            process.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    stdOut.AppendLine(e.Data);
                }
            };
            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    stdErr.AppendLine(e.Data);
                }
            };

            if (!process.Start())
            {
                return ServiceResult.Failure($"تعذّر تشغيل الأداة: {Path.GetFileName(fileName)}");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var detail = stdErr.Length > 0 ? stdErr.ToString().Trim() : stdOut.ToString().Trim();
                if (detail.Length > 600)
                {
                    detail = detail[^600..];
                }

                return ServiceResult.Failure(
                    string.IsNullOrWhiteSpace(detail)
                        ? $"{Path.GetFileName(fileName)} انتهى برمز خطأ {process.ExitCode}."
                        : detail);
            }

            return ServiceResult.Success();
        }

        private static bool TryResolveTool(string exeName, int preferredMajor, out string fullPath)
        {
            fullPath = string.Empty;

            // Prefer tools that match the running server major version (avoids pg_dump 18 vs server 14).
            if (preferredMajor > 0)
            {
                foreach (var candidate in EnumerateInstallCandidates(exeName, preferredMajor))
                {
                    fullPath = candidate;
                    return true;
                }
            }

            var pgBin = Environment.GetEnvironmentVariable("PGBIN");
            if (!string.IsNullOrWhiteSpace(pgBin))
            {
                var candidate = Path.Combine(pgBin, exeName);
                if (File.Exists(candidate))
                {
                    fullPath = candidate;
                    return true;
                }
            }

            // Next: installed versions whose major is <= server major (closest first), then any newer.
            foreach (var candidate in EnumerateInstallCandidates(exeName, preferredMajor: null)
                         .OrderBy(path =>
                         {
                             var major = TryParseInstallMajor(path);
                             if (preferredMajor > 0 && major > 0)
                             {
                                 // Prefer not-newer-than-server, then closest.
                                 var newerPenalty = major > preferredMajor ? 1000 : 0;
                                 return newerPenalty + Math.Abs(major - preferredMajor);
                             }

                             return major > 0 ? -major : int.MaxValue;
                         }))
            {
                fullPath = candidate;
                return true;
            }

            var fromPath = FindOnPath(exeName);
            if (!string.IsNullOrEmpty(fromPath))
            {
                fullPath = fromPath;
                return true;
            }

            return false;
        }

        private static IEnumerable<string> EnumerateInstallCandidates(string exeName, int? preferredMajor)
        {
            foreach (var root in new[]
                     {
                         Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                         Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
                     })
            {
                if (string.IsNullOrEmpty(root))
                {
                    continue;
                }

                var pgRoot = Path.Combine(root, "PostgreSQL");
                if (!Directory.Exists(pgRoot))
                {
                    continue;
                }

                IEnumerable<string> versionDirs = Directory.GetDirectories(pgRoot);
                if (preferredMajor is int major)
                {
                    versionDirs = versionDirs.Where(dir =>
                        string.Equals(Path.GetFileName(dir), major.ToString(), StringComparison.OrdinalIgnoreCase));
                }

                foreach (var versionDir in versionDirs)
                {
                    var candidate = Path.Combine(versionDir, "bin", exeName);
                    if (File.Exists(candidate))
                    {
                        yield return candidate;
                    }
                }
            }
        }

        private static int TryParseInstallMajor(string toolPath)
        {
            // .../PostgreSQL/14/bin/pg_dump.exe
            var binDir = Path.GetDirectoryName(toolPath);
            var versionDir = Path.GetDirectoryName(binDir);
            var name = Path.GetFileName(versionDir);
            return int.TryParse(name, out var major) ? major : 0;
        }

        private static string? FindOnPath(string exeName)
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrWhiteSpace(pathEnv))
            {
                return null;
            }

            foreach (var segment in pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                var candidate = Path.Combine(segment.Trim().Trim('"'), exeName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static void SecureDeleteFile(string? path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return;
            }

            try
            {
                var info = new FileInfo(path);
                if (info.Length > 0)
                {
                    using var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None);
                    var zeros = new byte[Math.Min(8192, info.Length)];
                    var remaining = info.Length;
                    while (remaining > 0)
                    {
                        var chunk = (int)Math.Min(zeros.Length, remaining);
                        stream.Write(zeros, 0, chunk);
                        remaining -= chunk;
                    }

                    stream.Flush(true);
                }

                File.Delete(path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PostgresBackupService.SecureDeleteFile: {ex.Message}");
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    // ignored — best-effort cleanup
                }
            }
        }
    }
}
