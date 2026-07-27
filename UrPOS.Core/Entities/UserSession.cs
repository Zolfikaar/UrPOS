using System;

namespace UrPOS.Core.Entities
{
    public class UserSession
    {
        private static UserSession? _instance;
        private static readonly object _lock = new object();

        // الخصائص المقفلة التي تمثل المستخدم الحالي
        public int? UserId { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string RoleName { get; private set; } = string.Empty;

        /// <summary>
        /// يُضبط إلى true بعد Start(...) عند دخول الضيف.
        /// </summary>
        public bool IsGuest { get; set; }

        public bool IsLoggedIn => UserId.HasValue || IsGuest;

        // constructor مخفي لمنع الإنشاء العشوائي
        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new UserSession();
                }
            }
        }

        // دالة بدء الجلسة للمستخدم الطبيعي
        public void Start(int userId, string username, string fullName, string roleName)
        {
            UserId = userId;
            UserName = username;
            FullName = fullName;
            RoleName = string.IsNullOrWhiteSpace(roleName) ? "Cashier" : roleName;
            IsGuest = false;
        }

        public void Clear()
        {
            UserId = null;
            UserName = string.Empty;
            FullName = string.Empty;
            RoleName = string.Empty;
            IsGuest = false;
        }
    }
}
