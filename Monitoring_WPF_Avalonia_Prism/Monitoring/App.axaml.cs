using AutoMapper;
using Avalonia;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Monitoring.Infrastructure;
using Monitoring.Sharing.Interfaces;
using Monitoring.ViewModels;
using Monitoring.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using Prism.Regions;
using Monitoring.Tools;

namespace Monitoring;

public partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Required when overriding Initialize
        base.Initialize();
    }

    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IAppConfig>(() => new AppConfig(Path.Combine(AppContext.BaseDirectory, "config.json")));

        containerRegistry.RegisterForNavigation<DashBoardView, DashBoardVM>();
        containerRegistry.RegisterForNavigation<DetailView, DetailVM>();
        containerRegistry.RegisterForNavigation<TemplatesView, TemplatesVM>();

        containerRegistry.Register<IApiService, ApiService>();
        containerRegistry.RegisterSingleton<IMetricsPollingService, MetricsPollingService>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingTemplates>();
            cfg.AddProfile<MappingMertics>();
        });
        containerRegistry.RegisterInstance<IMapper>(mapperConfig.CreateMapper());
    }

    protected override void OnInitialized()
    {
        var regionManager = Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion("ContentRegion", typeof(DashBoardView));
    }
}
