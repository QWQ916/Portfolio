using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Metrics.Hosts
{
    public class ListHosts
    {
        [JsonPropertyName("items")]
        public List<HostItem>? Hosts { get; set; }

        [JsonPropertyName("total")]
        public int TotalCount { get; set; }
    }
}
