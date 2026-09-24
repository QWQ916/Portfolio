using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;

namespace Monitoring.Models.Widgets
{
    public class InfoWidgetVM : WidgetBase, IDisposable
    {
        public string Value { get => _value; set => SetProperty(ref _value, value); }
        string _value;

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, string> _valueSelector;
        public InfoWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, string> valueSelector)
        {
            _pollingService = pollingService;
            _valueSelector = valueSelector;

            _pollingService.MetricsUpdated += OnMetricsUpdated;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            Value = _valueSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|
    }
}
