using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Hosts
{
    public class ApiResponseHosts
    {
        [JsonPropertyName("node_list")]
        public ListHosts? HostsList { get; set; }
    }
}
