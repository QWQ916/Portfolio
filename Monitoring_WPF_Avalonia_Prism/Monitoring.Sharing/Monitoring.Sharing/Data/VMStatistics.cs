using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Data
{
    public class VMStatistics
    {
        public int? Running { get; set; }
        public int? Stopped { get; set; }
        public int? Suspended { get; set; }
        public int? Paused { get; set; }
        public int? Error { get; set; }
        public int? Total { get => Running + Stopped + Suspended + Paused + Error; }
    }
}
