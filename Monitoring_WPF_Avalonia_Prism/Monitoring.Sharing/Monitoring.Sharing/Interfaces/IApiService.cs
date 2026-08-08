using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Monitoring.Sharing.Data;


namespace Monitoring.Sharing.Interfaces;

public interface IApiService
{
    Task<List<OSTemplate>> GetTemplatesAsync(string endPoint);
    Task<MetricInfrastructure> GetMetricsInfrastructureAsync(string endPoint);
    Task<List<Cluster>> GetClustersAsync(string endPoint);
    Task AuthAsync();
    Task<List<Host>> GetHostsAsync(string endPoint);
    Task<List<VirtualMachine>> GetVMsAsync(string endPoint);
}