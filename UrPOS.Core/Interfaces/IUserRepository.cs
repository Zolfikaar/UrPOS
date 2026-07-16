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
    }
}