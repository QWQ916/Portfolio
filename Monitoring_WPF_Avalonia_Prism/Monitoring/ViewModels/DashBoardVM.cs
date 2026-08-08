using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Models.Widgets;
using Monitoring.Sharing.Interfaces;
using Monitoring.ViewModels;
using Prism.Commands;
using Prism.Navigation;
using Prism.Regions;
using Monitoring.Infrastructure;
using Monitoring.Sharing.Data;
using System.Diagnostics;
using System.Threading;

namespace Monitoring.ViewModels
{
    public class DashBoardVM : ViewModelBase
    {
        public ObservableCollection<WidgetBase> Widgets { get; } = new();
        public DelegateCommand<WidgetBase> OpenWidgetCommand { get; }
        private readonly PeriodicTimer _periodicTimer;
        private readonly IMetricsPollingService _pollingService;

        private readonly IAppConfig _config;

        public MetricInfrastructure Metrics { get => _metric;
            set => SetProperty(ref _metric, value); }
        MetricInfrastructure _metric = new();

        private bool _isLoading = true;
        public bool IsLoading { get => _isLoading; set => SetProperty(ref _isLoading, value); }

        private readonly IRegionManager _regionManager;
        private readonly IApiService _apiService;

        public DashBoardVM(IRegionManager regionManager, IApiService api, IAppConfig config, IMetricsPollingService pollingService)
        {
            _regionManager = regionManager;
            _config = config;
            _apiService = api;
            _pollingService = pollingService;

            OpenWidgetCommand = new DelegateCommand<WidgetBase>(OpenWidget);
            LoadMetricsAsync();
            // Черновик данных "Пустышки"
            //Widgets.Add(new InfoWidgetVM { ID = 1, Title = "Зарядка", Value = "56 %",           Row = 0, Column = 0 });
            //Widgets.Add(new StatusWidgetVM { ID = 2, Title = "Сервер", IsOn = true,             Row = 0, Column = 1 });
            //Widgets.Add(new InfoWidgetVM { ID = 3, Title = "Затраты", Value = "128 500 ₽",      Row = 0, Column = 2 });
            //Widgets.Add(new StatusWidgetVM { ID = 4, Title = "На месте", IsOn = false,          Row = 1, Column = 0 });

            //Widgets.Add(new ProgressWidgetVM { ID = 5, Title = "Потрачено всего ресурсов", Progress = 12,   Row = 1, Column = 1 });
            //Widgets.Add(new ProgressWidgetVM { ID = 6, Title = "Выполнено работы", Progress = 30,           Row = 1, Column = 2 });
            //Widgets.Add(new ProgressWidgetVM { ID = 7, Title = "Осталось дней лета", Progress = 58,         Row = 2, Column = 0 });
            //Widgets.Add(new ProgressWidgetVM { ID = 8, Title = "Устранено конкурентов", Progress = 74,      Row = 2, Column = 1 });
            //Widgets.Add(new ProgressWidgetVM { ID = 9, Title = "Удовлетворенность работой", Progress = 96,  Row = 2, Column = 2 });

            //Widgets.Add(new LineChartWidgetVM
            //{
            //    ID = 10,
            //    Title = "Сессии за день",
            //    Values = new double[] { 5, 8, 12, 20, 28, 30, 55, 45, 35, 22, 30, 33 },
            //    Labels = new string[] { "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00" },
            //    Row = 3,
            //    Column = 0,
            //    ColumnSpan = 3,
            //    Threshold = 40
            //});

            //Widgets.Add(new LineChartWidgetVM
            //{
            //    ID = 11,
            //    Title = "Процесс",
            //    Values = new double[] { 10, 23, 5 },
            //    Labels = new string[] { "05:30", "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00" },
            //    Row = 4,
            //    Column = 0,
            //    ColumnSpan = 2,
            //    Threshold = 20
            //});

            //Widgets.Add(new GaugeWidgetVM
            //{
            //    ID = 11,
            //    Title = "Загрузка ЦОД",
            //    Max = 6000,
            //    Value = 5170,
            //    Row = 4,
            //    Column = 2
            //});

            //Widgets.Add(new GaugeWidgetVM
            //{
            //    ID = 12,
            //    Title = "Загрузка ЦОД 1",
            //    Max = 6000,
            //    Value = 5170,
            //    YellowFrom = 5000,
            //    RedFrom = 5500,
            //    Row = 5,
            //    Column = 0
            //});

            //Widgets.Add(new GaugeWidgetVM
            //{
            //    ID = 13,
            //    Title = "Загрузка ЦОД 2",
            //    Max = 10000,
            //    Value = 9170,
            //    YellowFrom = 7000,
            //    RedFrom = 9000,
            //    Row = 5,
            //    Column = 1
            //});

            //Widgets.Add(new GaugeWidgetVM
            //{
            //    ID = 14,
            //    Title = "Загрузка ЦОД 3",
            //    Max = 5000,
            //    YellowFrom = 3000,
            //    RedFrom = 4000,
            //    Value = 5170,
            //    Row = 5,
            //    Column = 2
            //});

            //Widgets.Add(new BarChartWidgetVM
            //{
            //    ID = 15,
            //    Title = "Сессии по серверам",
            //    Data = new Dictionary<string, double>
            //    {
            //        { "Serv1", 5300 },
            //        { "Serv2", 5500 },
            //        { "Serv3", 5760 },
            //        { "Serv4", 5359 }
            //    },
            //    Row = 6,
            //    Column = 0,
            //    ColumnSpan = 2                
            //});

            // Реальные данные
            Widgets.Add(new GaugeWidgetVM
                (
                _pollingService,
                valueSelector: metric => Convert.ToDouble(metric.PartitionUsagePercent ?? 0),
                titleSelector: metric => $"Диск:\n{Math.Round((metric.PartitionUsageMB / Math.Pow(1024, 2)) ?? 0, 2)} / {Math.Round((metric.PartitionTotalMB / Math.Pow(1024, 2)) ?? 0, 2)} ТБ"
                )
            {
                ID = 1,
                Max = 100,
                YellowFrom = 60,
                RedFrom = 80,
                Row = 0,
                Column = 0,
                IsPercent = true,
                ColumnSpan = 1,
                Type = WidgetBase.MetricInfrType.Partition
            });

            Widgets.Add(new GaugeWidgetVM
                (
                _pollingService,
                valueSelector: metric => Convert.ToDouble(metric.MemoryUsedPercent ?? 0),
                titleSelector: metric => (metric.MemoryUsedMB == null || metric.MemoryUsedMB == 0 || metric.MemoryTotalMB == null || metric.MemoryTotalMB == 0) ? "Оперативка:\n" : $"Оперативка:\n{metric.MemoryUsedMB} / {metric.MemoryTotalMB} МБ"
                )
            {
                ID = 2,
                Max = 100,
                YellowFrom = 50,
                RedFrom = 70,
                Row = 0,
                Column = 1,
                IsPercent = true,
                ColumnSpan = 2,
                Type = WidgetBase.MetricInfrType.Memory
            });

            Widgets.Add(new GaugeWidgetVM
                (
                _pollingService,
                valueSelector: metric => Convert.ToDouble(metric.CPUUsagePercent ?? 0),
                titleSelector: metric => $"CPU:\n"
                )
            {
                ID = 3,
                Max = 100,
                YellowFrom = 50,
                RedFrom = 70,
                Row = 2,
                Column = 2,
                IsPercent = true,
                Type = WidgetBase.MetricInfrType.Cpu
            });

            Widgets.Add(new GaugeWidgetVM
                (
                _pollingService,
                valueSelector: metric => Convert.ToDouble(metric.SwapUsedPercent ?? 0),
                titleSelector: metric => (metric.SwapTotalMB == null || metric.SwapUsedMB == 0 || metric.SwapTotalMB == null || metric.SwapTotalMB == 0) ? "Swap-память:\n" : $"Swap-память:\n{metric.SwapUsedMB} / {metric.SwapTotalMB} МБ"
                )
            {
                ID = 2,
                Max = 100,
                YellowFrom = 50,
                RedFrom = 70,
                Row = 2,
                Column = 0,
                IsPercent = true,
                Type = WidgetBase.MetricInfrType.Swap
            });

            Widgets.Add(new ProgressWidgetVM(
                pollingService,
                progressSelector: m => 45,
                titleSelector: m => "Загруженность сети"
                )
            { ID = 4, Row = 2, Column = 1 });

            Widgets.Add(new InfoWidgetVM
                (
                _pollingService,
                valueSelector: metric => metric.ClustersTotal?.ToString() ?? "- нет данных -"
               )
            {
                ID = 5,
                Title = "\nВсего кластеров:",
                Row = 1, Column = 0,
                Type = WidgetBase.MetricInfrType.Cluster
            });

            Widgets.Add(new InfoWidgetVM
                (
                _pollingService,
                valueSelector: metric => metric.NodeStatistics.Online.ToString() + " из " + metric.NodeStatistics.Total ?? "- нет данных -"
               )
            {
                ID = 6,
                Title = "Хосты:\nОнлайн",
                Row = 1,
                Column = 1,
                Type = WidgetBase.MetricInfrType.Node
            });

            Widgets.Add(new InfoWidgetVM
                (
                _pollingService,
                valueSelector: metric => metric.VMStatistics.Running.ToString() + " из " + metric.VMStatistics.Total ?? "- нет данных -"
               )
            {
                ID = 7,
                Title = "ВМ:\nРаботают",
                Row = 1,
                Column = 2,
                Type = WidgetBase.MetricInfrType.Vm
            });

            Widgets.Add(new BarChartWidgetVM(
                _pollingService,
                dataSelector: m => new Dictionary<string, double>
                {
                    {"Свободно", Math.Round(((m.PartitionTotalMB - m.PartitionUsageMB) / Math.Pow(1024, 2)) ?? 0)},
                    {"Всего", Math.Round((m.PartitionTotalMB / Math.Pow(1024, 2)) ?? 0)},
                },
                titleSelector: m => "Использование памяти на Диске, ТБ"
                )
            {
                ID = 7,
                Row = 3,
                Column = 0,
                ColumnSpan = 1
            });

            Widgets.Add(new LineChartWidgetVM(
                _pollingService,
                valuesSelector: m => new double[] { 23, 54, 37, 30, 44, 45, 40, 15, 5 },
                labelsSelector: m => new string[] { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00" },
                titleSelector: m => "Сессии за день"
                )
            {
                ID = 8,
                Row = 3,
                Column = 1,
                ColumnSpan = 2
            });

            pollingService.Start();
        }

        private void OpenWidget(WidgetBase widget)
        {
            // упаковываем виджет в параметры навигации
            var parameters = new NavigationParameters
            {
                { "widget", widget },
                {"polService", _pollingService },
                {"api", _apiService },
                {"config", _config }
            };
            _regionManager.RequestNavigate("ContentRegion", "DetailView", parameters);
        }

        // Объявляем метрики
        private async void LoadMetricsAsync()
        {     
            try
            {
                var metric = await _apiService.GetMetricsInfrastructureAsync(_config.Get("EndPointMetrics"));
                Metrics = metric;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки метрик: {ex.Message}");
            }
        }
    }
}
