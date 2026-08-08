using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;

namespace Monitoring.Tests
{
    // Заглушка сервиса поллинга: событие есть, но сам ничего не опрашивает.
    public class FakePollingService : IMetricsPollingService
    {
        public event Action<MetricInfrastructure> MetricsUpdated;
        public void Start() { }
        public void Stop() { }
    }

    // Заглушка API: возвращает заранее заданные метрики, остальное — пустое.
    public class FakeApiService : IApiService
    {
        public MetricInfrastructure MetricToReturn { get; set; } = new MetricInfrastructure();

        public Task AuthAsync() => Task.CompletedTask;
        public Task<MetricInfrastructure> GetMetricsInfrastructureAsync(string endPoint) => Task.FromResult(MetricToReturn);
        public Task<List<OSTemplate>> GetTemplatesAsync(string endPoint) => Task.FromResult(new List<OSTemplate>());
        public Task<List<Cluster>> GetClustersAsync(string endPoint) => Task.FromResult(new List<Cluster>());
        public Task<List<Host>> GetHostsAsync(string endPoint) => Task.FromResult(new List<Host>());
        public Task<List<VirtualMachine>> GetVMsAsync(string endPoint) => Task.FromResult(new List<VirtualMachine>());
    }

    // Заглушка конфига: возвращает любую строку-эндпоинт.
    public class FakeAppConfig : IAppConfig
    {
        public string Get(string key) => "/fake/endpoint";
        public T Get<T>(string key) => default;
        public string GetOrDefault(string key, string defaultValue = null) => defaultValue;
    }
}
