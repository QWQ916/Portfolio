using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.VirtualMachines
{
    public class HostVMItem
    {
        [JsonPropertyName("node_id")]
        public int NodeID { get; set; }

        [JsonPropertyName("name")]
        public string? NodeName { get; set; }
    }
}
