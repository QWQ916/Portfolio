using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Infrastructure
{
    public class ParentMetric
    {
        [JsonPropertyName("metrics")]
        public MetricItem Metric { get; set; }

        [JsonPropertyName("clusters_count")]
        public int? Cluster { get; set; }

        [JsonPropertyName("node_statistics")]
        public NodeStatisticsItem NStat { get; set; }

        [JsonPropertyName("vm_statistics")]
        public VMStatisticsItem VMStat { get; set; }
    }
}
