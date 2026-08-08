using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Clusters
{
    public class ClusterList
    {
        [JsonPropertyName("items")]
        public List<ClusterItem>? Clusters { get; set; }

        [JsonPropertyName("total")]
        public int TotalCount { get; set; }
    }
}
