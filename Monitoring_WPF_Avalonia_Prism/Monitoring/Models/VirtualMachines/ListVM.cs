using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.VirtualMachines
{
    public class ListVM
    {
        [JsonPropertyName("items")]
        public List<VMItem>? VMs { get; set; }

        [JsonPropertyName("total")]
        public int TotalCount { get; set; }
    }
}
