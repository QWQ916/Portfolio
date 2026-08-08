using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.VirtualMachines
{
    public class ApiResponseVM
    {
        [JsonPropertyName("vm_list")]
        public ListVM? VMs { get; set; }
    }
}
