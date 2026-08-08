using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitoring.Models.Widgets;
using Monitoring.Sharing.Data;
using Monitoring.Sharing.Interfaces;
using Prism.Commands;
using Prism.Regions;

namespace Monitoring.ViewModels
{
    public class TemplatesVM : ViewModelBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IApiService _apiService;
        private readonly IAppConfig _config;

        public ObservableCollection<OSTemplate> templates = new();

        private string _countText;
        public string CountText { get => _countText; set => SetProperty(ref _countText, value); }

        public ObservableCollection<OSTemplate> Templates { get => templates; set => SetProperty(ref templates, value); }

        public void OnNavigatedTo(NavigationContext navigationContext) => _ = LoadAsync();
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public TemplatesVM(IRegionManager regionManager, IApiService api, IAppConfig config)
        {
            _apiService = api;
            _regionManager = regionManager;
            _config = config;
        }

        private async Task LoadAsync()
        {
            try
            {
                var list = await _apiService.GetTemplatesAsync(_config.Get("EndPointTemplates"));
                Templates.Clear();
                foreach (var v in list)
                {
                    Templates.Add(v);
                }

                CountText = $"Всего: {Templates[0].TotalCount}";
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Ошибка загрузки шаблонов: {ex.Message}");
            }
        }
    }
}
