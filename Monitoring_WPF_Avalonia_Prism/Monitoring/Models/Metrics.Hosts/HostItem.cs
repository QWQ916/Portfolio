using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Hosts
{
    public class HostItem
    {
        [JsonPropertyName("ip_address")]
        public string? IP { get; set; }

        [JsonPropertyName("os")]
        public string? OS { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("arch")]
        public string? Arch { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("sdk_status")]
        public string? SDKStatus { get; set; }

        [JsonPropertyName("cluster_name")]
        public string? ClusterName { get; set; }

        [JsonPropertyName("vstorage_cluster_name")]
        public string? StorageClusterName { get; set; }
    }
}
