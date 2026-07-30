using System;
using System.Collections.Generic;
using System.Text;

namespace UrPOS.Core.Entities
{
    public class AppConfigurations
    {
        // === 1. إعدادات قاعدة البيانات المحلية ===
        public string DbHost { get; set; } = "localhost";
        public int DbPort { get; set; } = 5432;
        public string DbName { get; set; } = "urpos_db";
        public string DbUsername { get; set; } = "postgres";
        public string DbPassword { get; set; } = "postgres"; // string.Empty;

        // === 2. إعدادات تشغيل النظام والهوية التجارية ===
        public string StoreName { get; set; } = string.Empty;
        public string StorePhone { get; set; } = string.Empty;
        public string StoreAddress { get; set; } = string.Empty;

        // نوع الموديول المفعل: (General, Supermarket, Pharmacy)
        public string SystemModuleType {  get; set; } = string.Empty;

        // === 3. إعدادات الأجهزة والطباعة ===
        //public string SystemModuleVersion { get; set;} = string.Empty;
        public string DefaultPrinterName { get; set; } = string.Empty;
        public bool AutoPrintInvoice { get; set; } = true;
        public string InvoiceFooterText {  get; set; } = "شكراً لزيارتكم!";

        // === 4. وضع التجربة (Demo Mode) ===
        public bool IsDemoModeEnabled { get; set; }
        public int DemoTrialDays { get; set; } = 6;
        public DateTime? DemoTrialStartedAtUtc { get; set; }

        /// <summary>Remaining whole days in the demo trial (never negative).</summary>
        public int GetDemoDaysRemaining()
        {
            if (!IsDemoModeEnabled)
            {
                return DemoTrialDays;
            }

            var started = DemoTrialStartedAtUtc ?? DateTime.UtcNow;
            var elapsedDays = (int)Math.Floor((DateTime.UtcNow - started).TotalDays);
            return Math.Max(0, DemoTrialDays - elapsedDays);
        }

        // دالة مساعدة لتوليد نص الاتصال بـ PostgreSQL تلقائياً بناءً على الخصائص
        public string GetConnectionString()
        {
            return $"Host={DbHost};Port={DbPort};Database={DbName};Username={DbUsername};Password={DbPassword};Maximum Pool Size=10;";
        }

    }
}
