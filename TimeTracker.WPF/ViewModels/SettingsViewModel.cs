namespace TimeTracker.WPF.ViewModels;
public class SettingsViewModel : BindableBase, INavigationAware
{
    private string? _billablePercentage;

    public SettingsViewModel()
    {
        SaveSettingsCommand = new DelegateCommand(OnSaveSettings);
    }

    public string? BillablePercentage
    {
        get => _billablePercentage;
        set => SetProperty(ref _billablePercentage, value);
    }

    public DelegateCommand SaveSettingsCommand { get; }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        BillablePercentage = ConfigurationManager.AppSettings["BillablePercentage"] ?? "84";
    }
    private void OnSaveSettings()
    {
        if (!ValidateSettings())
        {
            return;
        }

        UpdateAppSettings("BillablePercentage", BillablePercentage!);
    }

    private bool ValidateSettings()
    {
        if (BillablePercentage is null || !double.TryParse(BillablePercentage, out _))
        {
            return false;
        }

        return true;
    }

    private static void UpdateAppSettings(string key, string value)
    {
        try
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings[key] == null)
            {
                settings.Add(key, value);
            }
            else
            {
                settings[key].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
        catch (ConfigurationErrorsException)
        {
            Console.WriteLine("Error writing app settings");
        }
    }
}
