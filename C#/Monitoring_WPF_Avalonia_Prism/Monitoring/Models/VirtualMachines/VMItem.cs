using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Monitoring.Models.Metrics.Hosts;

namespace Monitoring.Models.VirtualMachines
{
    public class VMItem
    {
        [JsonPropertyName("hostname")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("ip_address")]
        public string? IP { get; set; }

        [JsonPropertyName("cores_count")]
        public int CoresCount { get; set; }

        [JsonPropertyName("guest_os_version")]
        public string? GuestOsVersion { get; set; }

        [JsonPropertyName("guest_os")]
        public string? GuestOs { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("ram_size")]
        public int RamSize { get; set; }

        [JsonPropertyName("node")]
        public HostVMItem? Node { get; set; }
    }
}
