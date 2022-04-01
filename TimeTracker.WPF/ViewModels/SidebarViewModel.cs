namespace TimeTracker.WPF.ViewModels;

public class SidebarViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private bool _isCalendarSelected;
    private bool _isSettingsSelected;
    private bool _isInsightsSelected;

    public SidebarViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        IsCalendarSelected = true;
    }

    public bool IsCalendarSelected
    {
        get => _isCalendarSelected;
        set
        {
            SetProperty(ref _isCalendarSelected, value);
            if (_isCalendarSelected)
            {
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(CalendarView));
            }
        }
    }

    public bool IsInsightsSelected
    {
        get => _isInsightsSelected;
        set
        {
            SetProperty(ref _isInsightsSelected, value);
            if (_isInsightsSelected)
            {
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(InsightsView));
            }
        }
    }

    public bool IsSettingsSelected
    {
        get => _isSettingsSelected;
        set
        {
            SetProperty(ref _isSettingsSelected, value);
            if (_isSettingsSelected)
            {
                _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(SettingsView));
            }
        }
    }
}