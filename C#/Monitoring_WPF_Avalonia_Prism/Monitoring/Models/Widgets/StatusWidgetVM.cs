using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;

namespace Monitoring.Models.Widgets
{
    public class StatusWidgetVM : WidgetBase, IDisposable
    {
        public bool IsOn { get => _isOn; set => SetProperty(ref _isOn, value); }
        bool _isOn = false;

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, bool> _checkedSelector;
        private readonly Func<MetricInfrastructure, string> _titleSelector;
        public StatusWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, bool> checkedSelector, Func<MetricInfrastructure, string> titleSelector)
        {
            _pollingService = pollingService;
            _pollingService.MetricsUpdated += OnMetricsUpdated;
            _checkedSelector = checkedSelector;
            _titleSelector = titleSelector;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            IsOn = _checkedSelector(metric);
            Title = _titleSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|
    }
}
