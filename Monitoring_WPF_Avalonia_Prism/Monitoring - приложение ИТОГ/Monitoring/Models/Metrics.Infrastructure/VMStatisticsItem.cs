using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Infrastructure
{
    public class VMStatisticsItem
    {
        [JsonPropertyName("RUNNING")]
        public int? Running { get; set; }

        [JsonPropertyName("STOPPED")]
        public int? Stopped { get; set; }

        [JsonPropertyName("SUSPENDED")]
        public int? Suspended { get; set; }

        [JsonPropertyName("PAUSED")]
        public int? Paused { get; set; }

        [JsonPropertyName("ERROR")]
        public int? Error { get; set; }
    }
}
