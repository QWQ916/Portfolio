using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;
using SkiaSharp;

namespace Monitoring.Models.Widgets
{
    public class BarChartWidgetVM : WidgetBase
    {

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, Dictionary<string, double>> _dataSelector;
        private readonly Func<MetricInfrastructure, string> _titleSelector;

        public BarChartWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, Dictionary<string, double>> dataSelector, Func<MetricInfrastructure, string> titleSelector)
        {
            _pollingService = pollingService;
            _pollingService.MetricsUpdated += OnMetricsUpdated;
            _titleSelector = titleSelector;
            _dataSelector = dataSelector;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            Data = _dataSelector(metric);
            Title = _titleSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|

        private IEnumerable<ISeries> _series = new ISeries[0];
        public IEnumerable<ISeries> Series
        {
            get => _series;
            private set => SetProperty(ref _series, value);
        }

        private Axis[] _xAxes = new Axis[0];
        public Axis[] XAxes
        {
            get => _xAxes;
            private set => SetProperty(ref _xAxes, value);
        }

        private Dictionary<string, double> _data;
        public Dictionary<string, double> Data
        {
            get => _data;
            set
            {
                _data = value;

                var keys = value.Keys.ToArray();
                var vals = value.Values.ToArray();

                var palette = new[]
                {
                    new SKColor(0x42, 0xA5, 0xF5), // синий
                    new SKColor(0x66, 0xBB, 0x6A), // зелёный
                    new SKColor(0xFF, 0xA7, 0x26), // оранжевый
                    new SKColor(0xEF, 0x53, 0x50), // красный
                    new SKColor(0xAB, 0x47, 0xBC), // фиолетовый
                };

                Series = keys.Select((k, i) => (ISeries)new ColumnSeries<double?>
                {
                    Name = k,
                    // значение только на своей позиции, остальные null
                    Values = keys.Select((_, j) => j == i ? (double?)vals[i] : null).ToArray(),
                    Fill = new SolidColorPaint(palette[i % palette.Length]),
                    MaxBarWidth = 100,            // толщина столбца
                    IgnoresBarPosition = true    // центрировать столбец в своей категории
                }).ToArray();

                XAxes = new Axis[]
                {
                    new Axis { Labels = keys }
                };
            }
        }
    }
}
