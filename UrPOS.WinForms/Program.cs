using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using UrPOS.Infrastructure.Data;
using UrPOS.Presentation;

namespace UrPOS.WinForms
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. بناء الـ Host وتجميع الـ DI Container
            var host = ServiceConfigurator.CreateHostBuilder().Build();

            // 2. تهيئة قاعدة البيانات تلقائياً عند أول تشغيل
            using (var scope = host.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
                await dbInitializer.InitializeDatabaseAsync();
            }

            // 3. تشغيل الشاشة الرئيسية مع حقن الخدمات تلقائياً
            // var mainForm = host.Services.GetRequiredService<MainForm>();
            // Application.Run(mainForm);

            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
        }
    }
}