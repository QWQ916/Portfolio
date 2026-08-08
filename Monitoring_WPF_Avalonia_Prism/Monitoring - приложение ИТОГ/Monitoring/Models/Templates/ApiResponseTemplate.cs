using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Models.Templates
{
    public class ApiResponseTemplate
    {
        [JsonPropertyName("template_list")]
        public TemplateListContainer TemplateList { get; set; }
    }
}
