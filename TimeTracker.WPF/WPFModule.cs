namespace TimeTracker.WPF;

public class WPFModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();

        regionManager.RegisterViewWithRegion(RegionNames.SidebarRegion, nameof(SidebarView));
        regionManager.RegisterViewWithRegion(RegionNames.ActivitiesRegion, nameof(ActivitiesView));
        regionManager.RegisterViewWithRegion(RegionNames.CalendarOverviewRegion, nameof(CalendarOverviewView));
        regionManager.RegisterViewWithRegion(RegionNames.CalendarDetailsRegion, nameof(CalendarDetailsView));
        regionManager.RegisterViewWithRegion(RegionNames.SettingsRegion, nameof(SettingsView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        var container = containerRegistry.GetContainer();
        container.RegisterDelegate<ServiceFactory>(r => r.Resolve);
        container.RegisterMany(new[] 
        { 
            typeof(IMediator).GetAssembly(), 
            typeof(IActivityService).GetAssembly() ,
            typeof(NotificationHandler).GetAssembly()
        }, Registrator.Interfaces);
        
        var mapper = CreateMappings();
        
        containerRegistry.RegisterInstance(mapper);
        containerRegistry.Register<ITimeTrackerDbContextFactory, TimeTrackerDbContextFactory>();
        containerRegistry.Register<IDateTimeProvider, DateTimeProvider>();
        containerRegistry.Register<IActivityService, ActivityService>();
        containerRegistry.Register<ITrackingService, TrackingService>();
        containerRegistry.Register<IExportService, ExportService>();
        
        containerRegistry.RegisterForNavigation<SidebarView>();
        containerRegistry.RegisterForNavigation<ActivitiesView>();
        containerRegistry.RegisterForNavigation<CalendarOverviewView>();
        containerRegistry.RegisterForNavigation<CalendarDetailsView>();
        containerRegistry.RegisterForNavigation<CalendarView>();
        containerRegistry.RegisterForNavigation<InsightsView>();
        containerRegistry.RegisterForNavigation<SettingsView>();
    }

    private static IMapper CreateMappings()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Tracking, TrackingDto>().ReverseMap();
            cfg.CreateMap<Activity, ActivityDto>().ReverseMap();
        });

        return new Mapper(config);
    }
}