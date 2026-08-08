using Prism.Commands;
using Prism.Regions;

namespace Monitoring.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager _regionManager;

    public MainWindowViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        Title = "Мониторинг";
        NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    public DelegateCommand<string> NavigateCommand { get; }

    private void Navigate(string viewName)
        => _regionManager.RequestNavigate("ContentRegion", viewName);
}
