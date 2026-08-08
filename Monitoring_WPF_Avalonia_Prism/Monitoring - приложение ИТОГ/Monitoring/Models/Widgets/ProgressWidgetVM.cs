using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;

namespace Monitoring.Models.Widgets
{
    public class ProgressWidgetVM : WidgetBase, IDisposable
    {

        // ВАЖНО, для автообновления! ------------------------------------|
        private readonly IMetricsPollingService _pollingService;
        private readonly Func<MetricInfrastructure, int> _progressSelector;
        private readonly Func<MetricInfrastructure, string> _titleSelector;
        public ProgressWidgetVM(IMetricsPollingService pollingService, Func<MetricInfrastructure, int> progressSelector, Func<MetricInfrastructure, string> titleSelector)
        {
            _pollingService = pollingService;
            _progressSelector = progressSelector;

            _pollingService.MetricsUpdated += OnMetricsUpdated;
            _titleSelector = titleSelector;
        }

        public void OnMetricsUpdated(MetricInfrastructure metric)
        {
            Progress = _progressSelector(metric);
            Title = _titleSelector(metric);
        }
        public void Dispose()
        {
            _pollingService.MetricsUpdated -= OnMetricsUpdated;
        }
        // Конец блока с автообновлениями ------------------------------|

        int _progress;
        public int Progress {
            get {
                return _progress;
            }
            set {
                if (value > 100)
                {
                    SetProperty(ref _progress, 100); _progress = 100;
                }
                else if (value < 0)
                {
                    SetProperty(ref _progress, 0); _progress = 0; 
                }
                else
                {
                    SetProperty(ref _progress, value); _progress = value; 
                }
            }
        }

        public string Color
        {
            get
            {
                if (_progress <= 20) return "Red";
                else if (_progress <= 40) return "Orange";
                else if (_progress <= 60) return "Yellow";
                else if (_progress <= 80) return "LightGreen";
                else return "Green";
            }
        }
    }
}
