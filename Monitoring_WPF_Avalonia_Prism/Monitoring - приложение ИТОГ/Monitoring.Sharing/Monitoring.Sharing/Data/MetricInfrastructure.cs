using Monitoring.Sharing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Monitoring.Sharing.Data
{
    public class MetricInfrastructure
    {
        // Метрики из второго вложенного metrics
        public int? PartitionUsagePercent { get; set; }

        public double? PartitionUsageMB { get; set; }

        public double? PartitionTotalMB { get; set; }

        public double? MemoryTotalMB { get; set; }

        public double? MemoryFreeMB { get; set; }

        public double? MemoryUsedMB { get; set; }

        public int? MemoryUsedPercent { get; set; }

        public int? CPUUsagePercent { get; set; }

        public double? SwapTotalMB { get; set; }

        public double? SwapFreeMB { get; set; }

        public double? SwapUsedMB { get; set; }

        public double? SwapUsedPercent { get; set; }

        // Кластеры
        public int? ClustersTotal { get; set; }

        // Статистика узлов
        public NodeStatistics NodeStatistics { get; set; }

        // Статистика виртуальных машин
        public VMStatistics VMStatistics { get; set; }
    }
}
