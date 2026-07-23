using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
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

            using (var scope = host.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                await dbInitializer.InitializeDatabaseAsync();
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
