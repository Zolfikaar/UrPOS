using System.Collections.Generic;
using System.Threading.Tasks;
using UrPOS.Core.Entities;

namespace UrPOS.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> AddAsync(User user, int roleId); // نمرر الـ roleId عند الإنشاء
        Task<bool> UpdateAsync(User user);
        Task<bool> UpdateStatusAsync(int userId, bool isActive); // تجميد أو تفعيل الحساب سريعاً
        Task<bool> HasAnyUsersAsync(); // للتحقق من وجود أي مستخدم في قاعدة البيانات
        Task<int> CreateAdminUserAsync(User user);

        /// <summary>
        /// يعيد مستخدم الضيف الافتراضي إن وُجد، أو ينشئ سجلاً خفيفاً بدور Cashier.
        /// </summary>
        Task<User> EnsureGuestUserAsync(string passwordHash);

        /// <summary>
        /// يحذف سجل الضيف من PostgreSQL (مع معالجة القيود المرتبطة).
        /// </summary>
        Task DeleteGuestUserAsync(int guestUserId);
    }
}
