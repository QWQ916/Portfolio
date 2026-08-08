using System.Collections.Generic;
using System.Drawing;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Linq;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;
using static ImTools.ImMap;

namespace Monitoring.Models.Widgets
{
    public class LineChartWidgetVM : WidgetBase
    {

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, IReadOnlyCollection<double>> _valuesSelector;
        private readonly Func<MetricInfrastructure, string> _titleSelector;
        private readonly Func<MetricInfrastructure, string[]> _labelsSelector;

        public LineChartWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, IReadOnlyCollection<double>> valuesSelector, Func<MetricInfrastructure, string> titleSelector, Func<MetricInfrastructure, string[]> labelsSelector)
        {
            _pollingService = pollingService;
            _pollingService.MetricsUpdated += OnMetricsUpdated;
            _valuesSelector = valuesSelector;
            _titleSelector = titleSelector;
            _labelsSelector = labelsSelector;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            Values = _valuesSelector(metric);
            Title = _titleSelector(metric);
            Labels = _labelsSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|

        private ISeries[] _series;
        public ISeries[] Series
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

        private IReadOnlyCollection<double> _values;
        public IReadOnlyCollection<double> Values
        {
            get => _values;
            set
            {
                _values = value;
                Series = new ISeries[]
                {
                    new LineSeries<double> { Values = value,
                                             Stroke = new SolidColorPaint(SKColors.Green, 7),           // Цвет и толщина линии
                                             Fill = new SolidColorPaint(SKColors.LightGreen),           // Заполнение области под графиком
                                             GeometrySize = 0,                                          // Размер точки
                                             GeometryStroke = new SolidColorPaint(SKColors.Black),      // Обводка точки
                                             GeometryFill = new SolidColorPaint(SKColors.DarkGreen)     // Заполнение точки
                    }
                };

                if (value.Count > 0)
                    MaxY = Math.Ceiling(value.Max() * 1.1);   // конечный потолок = максимум данных + 10%
            }
        }

        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                XAxes = new Axis[]
                {
                    new Axis { Labels = value }
                };
            }
        }
        // ------------------------------------ Поля для границ мин и макс + порог -------------------------------------
        private double? _threshold;
        public double? Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value; BuildSections();
            }
        }

        private RectangularSection[] _sections = new RectangularSection[0];
        public RectangularSection[] Sections { get => _sections; private set => SetProperty(ref _sections, value); }

        private double? _minY;
        public double? MinY { get => _minY; set { _minY = value; BuildYAxes(); } }

        private double? _maxY;
        public double? MaxY { get => _maxY; set { _maxY = value; BuildYAxes(); BuildSections(); } }

        private Axis[] _yAxes = new Axis[0];
        public Axis[] YAxes { get => _yAxes; private set => SetProperty(ref _yAxes, value); }

        private void BuildYAxes()
        {
            YAxes = new Axis[]
            {
        new Axis { MinLimit = _minY, MaxLimit = _maxY }
            };
        }

        private void BuildSections()
        {
            if (_threshold is null)
            {
                Sections = new RectangularSection[0];
                return;
            }

            Sections = new RectangularSection[]
            {
        new RectangularSection          // зона выше порога
        {
            Yi = _threshold,
            Yj = _maxY,
            Fill = new SolidColorPaint(SKColor.Parse("#22FF3B30"))
        },
        new RectangularSection          // линия на самом пороге
        {
            Yi = _threshold,
            Yj = _threshold,
            Stroke = new SolidColorPaint(SKColor.Parse("#FF3B30"), 2)
        }
            };
        }
    }
}
