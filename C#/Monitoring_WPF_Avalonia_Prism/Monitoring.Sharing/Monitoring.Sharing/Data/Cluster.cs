using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Data
{
    public class Cluster
    {
        public string? Name { get; set; }
        public int VMCount { get; set; }
        public int NodeCount { get; set; }
        public string? StorageName { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TotalCount { get; set; }
    }
}
