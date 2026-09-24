using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Templates
{
    public class TemplateItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("guest_os_version")]
        public string? GuestOsVersion { get; set; }

        [JsonPropertyName("creation_date")]
        public DateTime? CreationDate { get; set; }
    }
}
