using System;
using System.Text.Json;
using System.IO;
using UrPOS.Core.Entities;
using UrPOS.Core.Interfaces;

namespace UrPOS.Core.Interfaces
{
    public class JsonConfigurationService : IConfigurationService
    {
        private readonly string _filePath;
        private AppConfigurations? _cachedConfigurations;

        public JsonConfigurationService(string? customFilePath = null)
        {
            // تحديد مسار ملف الإعدادات المخصص أو افتراضياً في مجلد التطبيق الحالي
            _filePath = customFilePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            LoadConfigurations();
        }

        public AppConfigurations GetConfigurations()
        {
            return _cachedConfigurations ?? new AppConfigurations();
        }

        public bool SaveConfigrations(AppConfigurations configs)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true, // لتنسيق ملف الـ JSON
                };

                var jsonString = JsonSerializer.Serialize(configs, options);
                File.WriteAllText(_filePath, jsonString);

                // تحديث النسخة المحفوظة في الذاكرة الحية (Cache)
                _cachedConfigurations = configs;
                return true;

            }
            catch
            {
                return false;
            }
        }

        public void LoadConfigurations() 
        {
            try
            {
                if(File.Exists(_filePath))
                {
                    var jsonString = File.ReadAllText(_filePath);
                    _cachedConfigurations = JsonSerializer.Deserialize<AppConfigurations>(jsonString);
                }

                // إذا لم يكن الملف موجوداً، ننشئ إعدادات افتراضية ونحفظها لتوليد الملف تلقائياً
                if(_cachedConfigurations == null)
                {
                    _cachedConfigurations = new AppConfigurations();
                    SaveConfigrations(_cachedConfigurations);
                }
            }
            catch
            {
                // في حال وجود تلف في ملف الـ JSON، نعتمد خياراً آمناً
                _cachedConfigurations = new AppConfigurations();
            }
        }
    }
}
