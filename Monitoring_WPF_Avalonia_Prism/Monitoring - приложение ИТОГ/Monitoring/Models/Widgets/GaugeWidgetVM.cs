using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Drawing;
using Monitoring.Sharing.Interfaces;
using Monitoring.Sharing.Data;
using System;

namespace Monitoring.Models.Widgets
{
    public class GaugeWidgetVM : WidgetBase, IDisposable
    {
        bool _isPercent = false;
        public bool IsPercent { get => _isPercent; set => SetProperty(ref _isPercent, value); }

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, double> _valueSelector;
        private readonly Func<MetricInfrastructure, string> _titleSelector;
        public GaugeWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, double> valueSelector, Func<MetricInfrastructure, string> titleSelector)
        {
            _pollingService = pollingService;
            _pollingService.MetricsUpdated += OnMetricsUpdated;
            _valueSelector = valueSelector;
            _titleSelector = titleSelector;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            Value = _valueSelector(metric);
            Title = _titleSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|

        public double Max { get; set; } = 100;

        private double _yellowFrom = double.MaxValue;
        public double YellowFrom { get => _yellowFrom; set { _yellowFrom = value; BuildGauge(); } }

        private double _redFrom = double.MaxValue;
        public double RedFrom { get => _redFrom; set { _redFrom = value; BuildGauge(); } }

        private IEnumerable<ISeries> _series = new ISeries[0];
        public IEnumerable<ISeries> Series
        {
            get => _series;
            private set => SetProperty(ref _series, value);
        }

        private double _value;
        public double Value
        {
            get => _value;
            set { _value = value; BuildGauge(); }
        }

        private void BuildGauge()
        {
            var color =
                _value >= _redFrom ? new SKColor(0xE5, 0x3E, 0x3E) :
                _value >= _yellowFrom ? new SKColor(0xF5, 0xA6, 0x23) :
                                        new SKColor(0x3B, 0xB2, 0x73);

            Series = GaugeGenerator.BuildSolidGauge(
                new GaugeItem(_value, s =>
                {
                    s.Fill = new SolidColorPaint(color);
                    s.InnerRadius = 55;
                    if (IsPercent) s.DataLabelsFormatter = point => $"{point.Coordinate.PrimaryValue} %";
                    if (_value == 0) s.DataLabelsFormatter = point => $"-";
                    s.OuterRadiusOffset = 20;
                    s.DataLabelsSize = 34;
                    s.DataLabelsPaint = new SolidColorPaint(color);
                    s.DataLabelsPosition = PolarLabelsPosition.ChartCenter;
                }),
                new GaugeItem(GaugeItem.Background, s =>
                {
                    s.InnerRadius = 55;
                    s.OuterRadiusOffset = 20;
                    s.Fill = new SolidColorPaint(new SKColor(230, 230, 230));
                }));
        }

        // Внутренняя дуга-зоны: зелёная/жёлтая/красная секции
        public double GreenSize => Positive(_yellowFrom);
        public double YellowSize => Positive(_redFrom - _yellowFrom);
        public double RedSize => Positive(Max - _redFrom);

        private static double Positive(double v) => v > 0 ? v : 0;

        // Цвет заднего фона
        public string BackColor {
            get
            {
                var color =
                _value >= _redFrom ? "#FECACA" :
                _value >= _yellowFrom ? "#FFF9C4" :
                                        "#F2F4F7";
                return color;
            }
        }
    }
}
