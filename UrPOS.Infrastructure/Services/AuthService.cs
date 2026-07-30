using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public Tuple<IUserRepository, IPasswordHasher> Services => Tuple.Create(_userRepository, _passwordHasher);

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            // 1. جلب المستخدم من قاعدة البيانات بواسطة الـ Repository
            var user = await _userRepository.GetByUsernameAsync(username);

            // 2. التحقق من وجود المستخدم وصلاحية حسابه
            if (user == null || !user.IsActive)
            {
                return false;
            }

            // 3. التحقق من تطابق كلمة المرور عبر الـ Hasher
            var isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return false;
            }

            // 4. إذا كانت البيانات صحيحة، نطلق الجلسة الحية في الذاكرة للمشروع بالكامل
            UserSession.Instance.Start(
                user.Id,
                user.Username,
                user.FullName,
                user.RoleName ?? "Cashier" // افتراضي في حال عدم تحديد دور
            );

            return true;
        }

        public async Task<bool> LoginAsGuestAsync()
        {
            // كلمة مرور عشوائية — الضيف لا يسجّل دخولاً عادياً بكلمة مرور
            var passwordHash = _passwordHasher.HashPassword(Guid.NewGuid().ToString("N"));
            var guest = await _userRepository.EnsureGuestUserAsync(passwordHash);

            UserSession.Instance.Start(
                guest.Id,
                guest.Username,
                "زائر تجريبي",
                "Admin");
            UserSession.Instance.IsGuest = true;
            UserSession.Instance.IsDemoMode = true;

            return true;
        }

        public async Task CleanupGuestSessionAsync()
        {
            if (UserSession.Instance.IsGuest && UserSession.Instance.UserId.HasValue)
            {
                var guestId = UserSession.Instance.UserId.Value;
                await _userRepository.DeleteGuestUserAsync(guestId);
                UserSession.Instance.Clear();
            }
            else if (UserSession.Instance.IsGuest)
            {
                UserSession.Instance.Clear();
            }
        }

        public void Logout()
        {
            // تصفير الجلسة تماماً من الذاكرة (للمستخدم العادي)
            UserSession.Instance.Clear();
        }
    }
}
