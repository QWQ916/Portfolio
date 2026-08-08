using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Clusters
{
    public class ApiResponseCluster
    {
        [JsonPropertyName("cluster_list")]
        public ClusterList ClusterList { get; set; }
    }
}
