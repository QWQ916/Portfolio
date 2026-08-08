using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Sharing;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;
using System.Collections.ObjectModel;
using System.Net;
using AutoMapper;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Monitoring.Models.Templates;
using Monitoring.Models.Metrics;
using Monitoring.Models.Metrics.Infrastructure;
using Monitoring.Models.Metrics.Clusters;
using Monitoring.Models.Metrics.Hosts;
using Monitoring.Models.VirtualMachines;

namespace Monitoring.Infrastructure
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;
        private HttpResponseMessage _authResponse;
        private readonly CookieContainer _cookieContainer = new CookieContainer();
        private readonly IMapper _mapper;

        private readonly IAppConfig _config;

        private readonly string _loginUrl;

        public ApiService(IMapper mapper, IAppConfig config)
        {
            // 0. Натсройка маппера для сопоставления свойств
            _mapper = mapper;
            _config = config;
            _loginUrl = _config.Get("LoginUrl");

            // 1. Настройка обработчика: отключаем проверку SSL и включаем автоматическую работу с Cookie (аналог -SessionVariable)
            _handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = _cookieContainer,
            };

            // 1.1. Настройка клиента
            _client = new HttpClient(_handler)
            {
                BaseAddress = new Uri(_config.Get("ApiBaseUrl")),
                Timeout = TimeSpan.FromSeconds(15)
            };
            _config = config;
        }

        public async Task AuthAsync()
        {
            // 2. Формируем тело для авторизации (аналог $AuthBody)
            var authBody = new
            {
                login = _config.Get("Login"),
                password = _config.Get("Password")
            };

            string jsonBody = JsonSerializer.Serialize(authBody);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            try
            {
                // 3. Выполняем POST-запрос для авторизации
                Debug.WriteLine("Авторизация...");
 
                HttpResponseMessage response = await _client.PostAsync(_loginUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Login Successful!");
                    string responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\nОшибка при выполнении запросов: {ex.Message}\n");
            }
        }

        public async Task<List<OSTemplate>> GetTemplatesAsync(string endPoint)
        {
            await AuthAsync();

            var resp = await _client.GetAsync(endPoint);
            var data = await resp.Content.ReadFromJsonAsync<ApiResponseTemplate>();

            if (data?.TemplateList?.Templates != null)
            {
                var temp = _mapper.Map<List<OSTemplate>>(data.TemplateList.Templates);
                foreach (var t in temp) { t.TotalCount = data.TemplateList.TotalCount; }

                return temp;
            }
            else
            {
                Debug.WriteLine("Шаблоны не найдены или структура ответа пуста.");
                return new List<OSTemplate>();
            }
        }

        public async Task<MetricInfrastructure> GetMetricsInfrastructureAsync(string endPoint)
        {
            await AuthAsync();

            var resp = await _client.GetAsync(endPoint);

            var data = await resp.Content.ReadFromJsonAsync<ApiResponseMetric>();

            if (data?.Metric != null)
            {
                var metric = _mapper.Map<MetricInfrastructure>(data.Metric.Metric);
                metric.ClustersTotal = data.Metric.Cluster;

                metric.VMStatistics = _mapper.Map<VMStatistics>(data.Metric.VMStat);
                metric.NodeStatistics = _mapper.Map<NodeStatistics>(data.Metric.NStat);
                return metric;
                
            }
            else
            {
                Debug.WriteLine("Данных нет или структура ответа пуста.");
                return new MetricInfrastructure();
            }
        }

        public async Task<List<Cluster>> GetClustersAsync(string endPoint)
        {
            await AuthAsync();

            var resp = await _client.GetAsync(endPoint);

            var data = await resp.Content.ReadFromJsonAsync<ApiResponseCluster>();

            if (data?.ClusterList?.Clusters != null)
            {
                var cluster = _mapper.Map<List<Cluster>>(data.ClusterList.Clusters);
                foreach (var c in cluster) { c.TotalCount = data.ClusterList.TotalCount; }

                return cluster;
            }
            else
            {
                Debug.WriteLine("Данных нет или структура ответа пуста.");
                return new List<Cluster>();
            }
        }

        public async Task<List<Host>> GetHostsAsync(string endPoint)
        {
            await AuthAsync();

            var resp = await _client.GetAsync(endPoint);
            string responseString = await resp.Content.ReadAsStringAsync();

            var data = await resp.Content.ReadFromJsonAsync<ApiResponseHosts>();

            if (data?.HostsList?.Hosts != null)
            {
                var host = _mapper.Map<List<Host>>(data.HostsList.Hosts);
                foreach (var h in host) { h.TotalCount = data.HostsList.TotalCount; }

                return host;
            }
            else
            {
                Debug.WriteLine("Данных нет или структура ответа пуста.");
                return new List<Host>();
            }
        }

        public async Task<List<VirtualMachine>> GetVMsAsync(string endPoint)
        {
            await AuthAsync();

            var resp = await _client.GetAsync(endPoint);
            string responseString = await resp.Content.ReadAsStringAsync();

            var data = await resp.Content.ReadFromJsonAsync<ApiResponseVM>();

            if (data?.VMs?.VMs != null)
            {
                var vm = _mapper.Map<List<VirtualMachine>>(data.VMs.VMs);
                foreach (var v in vm) { v.TotalCount = data.VMs.TotalCount; }

                return vm;

            }
            else
            {
                Debug.WriteLine("Данных нет или структура ответа пуста.");
                return new List<VirtualMachine>();
            }
        }
    }
}
