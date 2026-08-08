using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Clusters
{
    public class ClusterItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("vm_count")]
        public int VMCount { get; set; }

        [JsonPropertyName("node_count")]
        public int NodeCount { get; set; }

        [JsonPropertyName("storage_name")]
        public string? StorageName { get; set; }

        [JsonPropertyName("created")]
        public DateTime createdDate { get; set; }
    }
}
