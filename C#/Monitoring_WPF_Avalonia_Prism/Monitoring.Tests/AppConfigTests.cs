using System;
using System.Collections.Generic;
using System.IO;
using Monitoring.Tools;
using Xunit;

namespace Monitoring.Tests
{
    public class AppConfigTests : IDisposable
    {
        private readonly string _path;

        public AppConfigTests()
        {
            // временный конфиг для теста
            _path = Path.Combine(Path.GetTempPath(), $"cfg_{Guid.NewGuid():N}.json");
            File.WriteAllText(_path, "{\"ApiBaseUrl\":\"http://localhost:5005/\",\"Timeout\":\"30\"}");
        }

        // Тест 4: AppConfig читает ключи, конвертирует и корректно обрабатывает отсутствующие
        [Fact]
        public void AppConfig_читает_ключи_и_обрабатывает_отсутствующие()
        {
            var cfg = new AppConfig(_path);

            Assert.Equal("http://localhost:5005/", cfg.Get("ApiBaseUrl"));   // существующий ключ
            Assert.Equal(30, cfg.Get<int>("Timeout"));                       // конвертация в int
            Assert.Equal("деф", cfg.GetOrDefault("НетКлюча", "деф"));        // значение по умолчанию
            Assert.Throws<KeyNotFoundException>(() => cfg.Get("НетКлюча"));   // отсутствующий → исключение
        }

        public void Dispose() => File.Delete(_path);
    }
}
