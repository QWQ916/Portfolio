using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Data
{
    public class OSTemplate
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? GuestOsVersion { get; set; }

        public DateTime? CreationDate { get; set; }

        public int TotalCount { get; set; }
    }
}
