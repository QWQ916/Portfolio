using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Infrastructure
{
    public class ApiResponseMetric
    {
        [JsonPropertyName("metrics")]
        public ParentMetric Metric { get; set; }
    }
}
