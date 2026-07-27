using System;

namespace UrPOS.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // لتخزين كلمة المرور المشفرة
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        // سنحتاج لاحقاً لجلب دور المستخدم (مثلاً: Admin, Cashier)
        public string? RoleName { get; set; }
    }
}