using AutoMapper;
using Monitoring.Models.Metrics.Infrastructure;
using Monitoring.Models.VirtualMachines;
using Monitoring.Sharing.Data;
using Monitoring.Tools;
using Xunit;

namespace Monitoring.Tests
{
    public class MappingTests
    {
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingTemplates>();
                cfg.AddProfile<MappingMertics>();
            });
            return config.CreateMapper();
        }

        // Тест 5: DTO метрик маппится в доменную модель
        [Fact]
        public void MetricItem_маппится_в_MetricInfrastructure()
        {
            var mapper = CreateMapper();

            var dto = new MetricItem
            {
                PartitionUsagePercent = 34,
                CPUUsagePercent = 67,
                MemoryUsedPercent = 85,
                PartitionTotalMB = 147111626.39
            };

            var result = mapper.Map<MetricInfrastructure>(dto);

            Assert.Equal(34, result.PartitionUsagePercent);
            Assert.Equal(67, result.CPUUsagePercent);
            Assert.Equal(85, result.MemoryUsedPercent);
            Assert.Equal(147111626.39, result.PartitionTotalMB);
        }

        // Тест 6: VMItem маппится в VirtualMachine, включая данные узла (ForMember) и перевод статуса
        [Fact]
        public void VMItem_маппится_в_VirtualMachine_с_данными_узла()
        {
            var mapper = CreateMapper();

            var dto = new VMItem
            {
                Name = "vm-app-01",
                Status = "RUNNING",
                CoresCount = 8,
                Node = new HostVMItem { NodeID = 5, NodeName = "node-05" }
            };

            var vm = mapper.Map<VirtualMachine>(dto);

            Assert.Equal("vm-app-01", vm.Name);
            Assert.Equal(8, vm.CoresCount);
            Assert.Equal(5, vm.NodeID);            // взято из Node.NodeID
            Assert.Equal("node-05", vm.NodeName);  // взято из Node.NodeName
            Assert.Equal("Запущено", vm.Status);   // сеттер перевёл RUNNING → Запущено
        }
    }
}
