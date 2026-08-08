using Prism.Commands;
using System;
using Prism.Navigation;
using Prism.Regions;
using Monitoring.Models.Widgets;
using Monitoring.Sharing.Interfaces;
using Monitoring.Sharing.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData.Aggregation;

namespace Monitoring.ViewModels;

public class DetailVM : ViewModelBase, INavigationAware, IRegionMemberLifetime
{
    public bool KeepAlive => false;

    private string _countText;
    public string CountText { get => _countText; set => SetProperty(ref _countText, value); }

    private readonly IRegionManager _regionManager;
    private IMetricsPollingService _pollingService;
    private WidgetBase _widget;

    private IApiService _apiService;
    private IAppConfig _config;

    public bool IsCluster => SelectedData is List<Cluster> or IEnumerable<Cluster>;
    public bool IsHost => SelectedData is List<Host> or IEnumerable<Host>;
    public bool IsVM => SelectedData is List<VirtualMachine> or IEnumerable<VirtualMachine>;

    private object? _selectedData;
    public object? SelectedData
    {
        get => _selectedData;
        set
        {
            if (SetProperty(ref _selectedData, value))
            {
                RaisePropertyChanged(nameof(IsCluster));
                RaisePropertyChanged(nameof(IsHost));
                RaisePropertyChanged(nameof(IsVM));
            }
        }
    }

    public ObservableCollection<Cluster> Clusters { get; } = new();
    public ObservableCollection<Host> Hosts { get; } = new();
    public ObservableCollection<VirtualMachine> VMs { get; } = new();

    public DetailVM(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        BackCommand = new DelegateCommand(GoBack);
    }

    public DelegateCommand BackCommand { get; }

    private string _info;
    public string Info { get => _info; set => SetProperty(ref _info, value); }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _widget = navigationContext.Parameters.GetValue<WidgetBase>("widget");
        Title = _widget.Title;
        _pollingService = navigationContext.Parameters.GetValue<IMetricsPollingService>("polService");
        _apiService = navigationContext.Parameters.GetValue<IApiService>("api");
        _config = navigationContext.Parameters.GetValue<IAppConfig>("config");

        BuildAsync();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }

    private void GoBack()
        => _regionManager.RequestNavigate("ContentRegion", "DashBoardView");

    public async void BuildAsync()
    {
        SelectedData = _widget.Type switch
        {
            WidgetBase.MetricInfrType.Cluster => Clusters,
            WidgetBase.MetricInfrType.Node => Hosts,
            WidgetBase.MetricInfrType.Vm => VMs,
            _ => null
        };

        var clusters = await _apiService.GetClustersAsync(_config.Get("EndPointClusters"));
        Clusters.Clear();
        foreach (var v in clusters)
        {
            Clusters.Add(v);
        }

        var hosts = await _apiService.GetHostsAsync(_config.Get("EndPointHosts"));
        Hosts.Clear();
        foreach (var v in hosts)
        {
            Hosts.Add(v);
        }

        var vms = await _apiService.GetVMsAsync(_config.Get("EndPointVM"));
        VMs.Clear();
        foreach (var v in vms)
        {
            VMs.Add(v);
        }

        if (IsCluster)
        {
            CountText = $"Всего: {Clusters[0].TotalCount}";
        }

        if (IsHost)
        {
            CountText = $"Всего: {Hosts[0].TotalCount}";
        }

        if (IsVM)
        {
            CountText = $"Всего: {VMs[0].TotalCount}";
        }

    }
}
