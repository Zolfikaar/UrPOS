using System.Threading.Tasks;

namespace UrPOS.Core.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        void Logout();
    }
}