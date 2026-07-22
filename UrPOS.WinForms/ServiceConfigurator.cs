using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;
using UrPOS.Infrastructure.Data;
using UrPOS.Infrastructure.Repositories;
using UrPOS.Infrastructure.Security;
using UrPOS.Infrastructure.Services;
using UrPOS.WinForms.Forms;

namespace UrPOS.Presentation
{
    public static class ServiceConfigurator
    {
        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // 1. تسجيل كائن الإعدادات والاتصال كـ Singleton (نسخة واحدة بالذاكرة)
                    services.AddSingleton<IConfigurationService, JsonConfigurationService>();
                    services.AddSingleton(sp => sp.GetRequiredService<IConfigurationService>().GetConfigurations());
                    services.AddSingleton<DbConnectionFactory>();

                    // 2. تسجيل الأدوات الأمنية والتهيئة
                    services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
                    services.AddTransient<DbInitializer>();

                    // 3. تسجيل المستودعات (Repositories)
                    services.AddTransient<IUserRepository, UserRepository>();
                    services.AddTransient<IProductRepository, ProductRepository>();
                    services.AddTransient<ISupplierRepository, SupplierRepository>();
                    services.AddTransient<IInventoryRepository, InventoryRepository>();
                    services.AddTransient<IInvoiceRepository, InvoiceRepository>();

                    // 4. تسجيل طبقة الخدمات (Services)
                    services.AddTransient<IAuthService, AuthService>();
                    services.AddTransient<IProductService, ProductService>();
                    services.AddTransient<IInvoiceService, InvoiceService>();

                    // 5. تسجيل نماذج العرض (WinForms)
                    services.AddTransient<LoginForm>();
                    services.AddTransient<MainForm>();
                    services.AddTransient<PosSalesForm>();
                });
        }
    }
}
