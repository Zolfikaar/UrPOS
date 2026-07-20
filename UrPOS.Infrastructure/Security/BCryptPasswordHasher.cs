using BCrypt.Net;
using UrPOS.Core.Interfaces;

namespace UrPOS.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        // Work Factor = 10 هو التوازن المثالي بين الأمان العالي جداً والسرعة على الأجهزة الضعيفة
        private const int WorkFactor = 10;

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // في حال وجود صيغة تشفير قديمة أو تالفة في قاعدة البيانات لتجنب كسر التطبيق
                return false;
            }
        }
    }
}