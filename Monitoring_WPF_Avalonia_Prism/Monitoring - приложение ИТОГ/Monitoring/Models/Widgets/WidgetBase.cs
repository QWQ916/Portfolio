using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Monitoring.Models.Widgets
{
    public abstract class WidgetBase : BindableBase
    {
        public int ID { get; set; }
        public MetricInfrType Type { get; set; }

        public string Title { get => _title; set=> SetProperty(ref _title, value); }
        string _title;

        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; } = 1;

        public enum MetricInfrType
        {
            Cpu,
            Memory,
            Vm,
            Node,
            Partition,
            Cluster,
            Swap
        }
    }
}
