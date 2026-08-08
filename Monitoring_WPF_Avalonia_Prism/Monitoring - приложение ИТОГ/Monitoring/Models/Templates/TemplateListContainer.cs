using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Templates
{
    public class TemplateListContainer
    {
        [JsonPropertyName("items")]
        public List<TemplateItem> Templates { get; set; }

        [JsonPropertyName("total")]
        public int TotalCount { get; set; }
    }
}
