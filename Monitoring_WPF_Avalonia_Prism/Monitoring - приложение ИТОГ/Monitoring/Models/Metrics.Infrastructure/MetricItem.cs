using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Monitoring.Models.Metrics.Infrastructure
{
    public class MetricItem
    {
        [JsonPropertyName("combined_partition_usage_percent")]
        public int? PartitionUsagePercent { get; set; }

        [JsonPropertyName("combined_partition_usage_mb")]
        public double? PartitionUsageMB { get; set; }

        [JsonPropertyName("combined_partition_total_mb")]
        public double? PartitionTotalMB { get; set; }

        [JsonPropertyName("combined_memory_total_mb")]
        public double? MemoryTotalMB { get; set; }

        [JsonPropertyName("combined_memory_free_mb")]
        public double? MemoryFreeMB { get; set; }

        [JsonPropertyName("combined_memory_used_mb")]
        public double? MemoryUsedMB { get; set; }

        [JsonPropertyName("combined_memory_used_percent")]
        public int? MemoryUsedPercent { get; set; }

        [JsonPropertyName("cpu_usage_percent")]
        public int? CPUUsagePercent { get; set; }

        [JsonPropertyName("combined_swap_total_mb")]
        public double? SwapTotalMB { get; set; }

        [JsonPropertyName("combined_swap_free_mb")]
        public double? SwapFreeMB { get; set; }

        [JsonPropertyName("combined_swap_used_mb")]
        public double? SwapUsedMB { get; set; }

        [JsonPropertyName("combined_swap_used_percent")]
        public double? SwapUsedPercent { get; set; }
    }
}
