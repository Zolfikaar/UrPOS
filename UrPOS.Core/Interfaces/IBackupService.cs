using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    /// <summary>
    /// Encrypted PostgreSQL database backup and restore.
    /// </summary>
    public interface IBackupService
    {
        Task<ServiceResult> CreateEncryptedBackupAsync(string destinationFilePath, string encryptionKey);
        Task<ServiceResult> RestoreEncryptedBackupAsync(string sourceFilePath, string encryptionKey);
    }
}
