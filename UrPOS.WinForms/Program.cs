using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;
using UrPOS.Presentation;
using UrPOS.WinForms.Forms;

namespace UrPOS.WinForms
{
    internal static class Program
    {
        /// <summary>
        /// Must remain synchronous. <c>async Task Main</c> resumes after awaits on an MTA
        /// thread-pool thread, which breaks OLE dialogs (SaveFileDialog / OpenFileDialog).
        /// </summary>
        [STAThread]
        static void Main()
        {
            // تفعيل مطابقة أسماء الأعمدة ذات الـ underscore تلقائياً مع C# PascalCase
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            ApplicationConfiguration.Initialize();

            var host = ServiceConfigurator.CreateHostBuilder().Build();

            // 1. تهيئة قواعد البيانات والجداول والأدوار فقط
            using (var scope = host.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                dbInitializer.InitializeDatabaseAsync().GetAwaiter().GetResult();
            }

            // 2. فحص هل يوجد أي مستخدم بالداتابيس؟ (يحدد فقط إن كان SetupForm مطلوباً)
            bool hasUsers;
            using (var scope = host.Services.CreateScope())
            {
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                hasUsers = userRepo.HasAnyUsersAsync().GetAwaiter().GetResult();
            }

            // 3. إذا لم يوجد مستخدمين، نفتح واجهة الإعداد لأول مرة (SetupForm)
            //    ملاحظة: دخول الضيف من SetupForm يضبط الجلسة ويُنشئ سجل guest — لا نُعيد فتح SetupForm لاحقاً بسبب IsGuest.
            if (!hasUsers)
            {
                using var setupForm = host.Services.GetRequiredService<SetupForm>();
                if (setupForm.ShowDialog() != DialogResult.OK)
                {
                    // إذا أغلق المستخدم النافذة دون إكمال الإعداد، يغلق البرنامج
                    return;
                }
            }

            // 4. حلقة تسجيل الدخول ← الشاشة الرئيسية (سواء للمستخدم أو للضيف)
            while (true)
            {
                // إذا كانت الجلسة نشطة مسبقاً (مثلاً دخول ضيف من SetupForm)، نتخطى LoginForm
                if (!UserSession.Instance.IsLoggedIn)
                {
                    using var loginForm = host.Services.GetRequiredService<LoginForm>();

                    // Guest Login و Login العادي يجب أن يعيدا DialogResult.OK
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        break;
                    }
                }

                // بمجرد وجود جلسة صالحة (عادية أو ضيف)، تفتح الشاشة الرئيسية
                using var mainForm = host.Services.GetRequiredService<MainForm>();
                Application.Run(mainForm);

                // إذا ضغط المستخدم "تسجيل الخروج"، ترجع Retry لتكرار الحلقة وفتح LoginForm مجدداً
                if (mainForm.DialogResult != DialogResult.Retry)
                {
                    break;
                }
            }
        }
    }
}
