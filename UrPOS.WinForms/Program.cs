using Microsoft.Extensions.DependencyInjection;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;
using UrPOS.Presentation;
using UrPOS.WinForms.Forms;

namespace UrPOS.WinForms
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            // تفعيل مطابقة أسماء الأعمدة ذات الـ underscore تلقائياً مع C# PascalCase
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            ApplicationConfiguration.Initialize();

            var host = ServiceConfigurator.CreateHostBuilder().Build();

            // 1. تهيئة قواعد البيانات والجداول والأدوار فقط
            using (var scope = host.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                await dbInitializer.InitializeDatabaseAsync();
            }

            // 2. فحص هل يوجد أي مستخدم بالداتابيس؟
            bool hasUsers;
            using (var scope = host.Services.CreateScope())
            {
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                hasUsers = await userRepo.HasAnyUsersAsync();
            }

            // 3. إذا لم يوجد مستخدمين، نفتح واجهة الإعداد لأول مرة (SetupWizardForm)
            if (!hasUsers)
            {
                using var setupForm = host.Services.GetRequiredService<SetupForm>();
                if (setupForm.ShowDialog() != DialogResult.OK)
                {
                    // إذا أغلق المستخدم النافذة دون إكمال الإعداد، يغلق البرنامج
                    return;
                }
            }

            // حلقة تسجيل الدخول ← الشاشة الرئيسية (مع إمكانية الخروج وإعادة الدخول)
            while (true)
            {
                using var loginForm = host.Services.GetRequiredService<LoginForm>();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    break;
                }

                using var mainForm = host.Services.GetRequiredService<MainForm>();
                Application.Run(mainForm);

                // Retry = المستخدم ضغط خروج لإعادة تسجيل الدخول
                if (mainForm.DialogResult != DialogResult.Retry)
                {
                    break;
                }
            }
        }
    }
}
