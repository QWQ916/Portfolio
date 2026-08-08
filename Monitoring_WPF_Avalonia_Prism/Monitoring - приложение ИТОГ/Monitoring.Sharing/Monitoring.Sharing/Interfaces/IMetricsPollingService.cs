using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;

namespace Monitoring.Sharing.Interfaces
{
    public interface IMetricsPollingService
    {
        event Action<MetricInfrastructure> MetricsUpdated;
        void Start();
        void Stop();
    }
}
