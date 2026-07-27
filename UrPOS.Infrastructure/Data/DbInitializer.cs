using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.Infrastructure.Data
{
    public class DbInitializer
    {
        private readonly AppConfigurations _configs;
        private readonly IPasswordHasher _passwordHasher;

        public DbInitializer(AppConfigurations configs, IPasswordHasher passwordHasher)
        {
            _configs = configs;
            _passwordHasher = passwordHasher;
        }

        public async Task InitializeDatabaseAsync()
        {
            // 1. التأكد من وجود قاعدة البيانات نفسها في سيرفر PostgreSQL
            await EnsureDatabaseExistsAsync();

            // 2. إنشاء الجداول والـ Indexes من سكريبت الـ Embedded SQL
            await ExecuteInitialSchemaScriptAsync();

            // 3. زراعة حساب المدير الافتراضي (Admin) إذا لم يكن موجوداً
            //await SeedDefaultAdminUserAsync();

            await SeedRolesAsync();

            // 4. مسح سجلات الضيف اليتيمة المتبقية بعد تعطل غير متوقع
            await CleanupOrphanGuestUsersAsync();
        }

        private async Task CleanupOrphanGuestUsersAsync()
        {
            using var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();

            // فك ارتباط أي فواتير تشير لسجلات guest ثم حذفها
            const string sql = @"
                UPDATE sales_invoices
                SET user_id = NULL
                WHERE user_id IN (SELECT id FROM users WHERE LOWER(username) = 'guest');

                UPDATE purchase_invoices
                SET user_id = NULL
                WHERE user_id IN (SELECT id FROM users WHERE LOWER(username) = 'guest');

                DELETE FROM user_roles
                WHERE user_id IN (SELECT id FROM users WHERE LOWER(username) = 'guest');

                DELETE FROM users
                WHERE LOWER(username) = 'guest';";

            await connection.ExecuteAsync(sql);
        }

        private async Task EnsureDatabaseExistsAsync()
        {
            // نفتح اتصالاً بقاعدة البيانات الافتراضية 'postgres' لمعرفة هل داتابيس urpos_db موجودة أم لا
            var masterConnectionString = $"Host={_configs.DbHost};Port={_configs.DbPort};Database=postgres;Username={_configs.DbUsername};Password={_configs.DbPassword};";

            using var connection = new NpgsqlConnection(masterConnectionString);
            await connection.OpenAsync();

            const string checkDbSql = "SELECT 1 FROM pg_database WHERE datname = @DbName;";
            var exists = await connection.ExecuteScalarAsync<int?>(checkDbSql, new { DbName = _configs.DbName });

            if (exists != 1)
            {
                // إنشاء قاعدة البيانات تلقائياً
                var createDbSql = $"CREATE DATABASE \"{_configs.DbName}\";";
                await connection.ExecuteAsync(createDbSql);
            }
        }

        private async Task ExecuteInitialSchemaScriptAsync()
        {
            using var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();

            // قراءة ملف سكريبت الـ SQL المدمج داخل الـ Assembly
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "UrPOS.Infrastructure.Data.Scripts.001_InitialSchema.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"لم يتم العثور على سكريبت التهيئة المدمج: {resourceName}");
            }

            using var reader = new StreamReader(stream);
            var sqlScript = await reader.ReadToEndAsync();

            // تنفيذ سكريبت الجداول
            await connection.ExecuteAsync(sqlScript);
        }

        private async Task SeedDefaultAdminUserAsync()
        {
            using var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();

            const string checkAdminSql = "SELECT COUNT(1) FROM users WHERE username = 'admin';";
            var adminCount = await connection.ExecuteScalarAsync<int>(checkAdminSql);

            if (adminCount == 0)
            {
                // تشفير كلمة المرور الافتراضية "admin123"
                var defaultPasswordHash = _passwordHasher.HashPassword("admin123");

                using var transaction = connection.BeginTransaction();
                try
                {
                    const string insertUserSql = @"
                        INSERT INTO users (username, password_hash, full_name, is_active)
                        VALUES ('admin', @PasswordHash, 'مدير النظام', TRUE)
                        RETURNING id;";

                    var userId = await connection.ExecuteScalarAsync<int>(insertUserSql, new { PasswordHash = defaultPasswordHash }, transaction);

                    // ربط كائن الأدمن بدور Admin (ID = 1)
                    const string insertRoleSql = "INSERT INTO user_roles (user_id, role_id) VALUES (@UserId, 1);";
                    await connection.ExecuteAsync(insertRoleSql, new { UserId = userId }, transaction);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task SeedRolesAsync() 
        {
            using var connection = new NpgsqlConnection(_configs.GetConnectionString());
            await connection.OpenAsync();
            const string checkRolesSql = "SELECT COUNT(1) FROM roles;";
            var rolesCount = await connection.ExecuteScalarAsync<int>(checkRolesSql);
            if (rolesCount == 0)
            {
                // إضافة الأدوار الافتراضية
                const string insertRolesSql = @"
                    INSERT INTO roles (name) VALUES 
                    ('Admin'),
                    ('Manager'),
                    ('Cashier');";
                await connection.ExecuteAsync(insertRolesSql);
            }
        }

    }
}