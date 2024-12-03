namespace TimeTracker.WPF.ViewModels;

public class SettingsViewModel : BindableBase, INavigationAware
{
    private string? _billablePercentage;
    private bool _showOnlyWorkingDays;
    private readonly ISettingsService _settingsService;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        SaveSettingsCommand = new DelegateCommand(OnSaveSettings);
    }

    public string? BillablePercentage
    {
        get => _billablePercentage;
        set => SetProperty(ref _billablePercentage, value);
    }

    public bool ShowOnlyWorkingDays
    {
        get { return _showOnlyWorkingDays; }
        set { _showOnlyWorkingDays = value; }
    }

    public DelegateCommand SaveSettingsCommand { get; }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) { }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        var settings = _settingsService.GetApplicationSettings();

        BillablePercentage = settings.BillablePercentage.ToString();
        ShowOnlyWorkingDays = settings.ShowOnlyWorkingDays;
    }

    private void OnSaveSettings()
    {
        if (!ValidateSettings())
        {
            return;
        }

        _settingsService.UpdateApplicationSettings(
            new ApplicationSettings(double.Parse(BillablePercentage!), ShowOnlyWorkingDays)
        );
    }

    private bool ValidateSettings()
    {
        if (BillablePercentage is null || !double.TryParse(BillablePercentage, out _))
        {
            return false;
        }

        return true;
    }
}
