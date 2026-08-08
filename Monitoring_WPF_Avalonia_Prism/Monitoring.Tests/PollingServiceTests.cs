using System.Threading.Tasks;
using Monitoring.Sharing.Data;
using Monitoring.Tools;
using Xunit;

namespace Monitoring.Tests
{
    public class PollingServiceTests
    {
        // Тест 7: Start запускает опрос и вызывает событие MetricsUpdated с данными
        [Fact]
        public async Task Start_вызывает_MetricsUpdated_с_данными()
        {
            var api = new FakeApiService
            {
                MetricToReturn = new MetricInfrastructure { PartitionUsagePercent = 34 }
            };
            var service = new MetricsPollingService(api, new FakeAppConfig());

            var tcs = new TaskCompletionSource<MetricInfrastructure>();
            service.MetricsUpdated += m => tcs.TrySetResult(m);

            service.Start();   // сразу делает первый опрос

            // ждём событие максимум 3 секунды
            var finished = await Task.WhenAny(tcs.Task, Task.Delay(3000));
            service.Stop();

            Assert.True(finished == tcs.Task, "Событие MetricsUpdated не сработало за 3 секунды");
            Assert.Equal(34, tcs.Task.Result.PartitionUsagePercent);
        }
    }
}
