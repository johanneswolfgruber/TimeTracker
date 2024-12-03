namespace TimeTracker.WPF.Services;

internal class SettingsService : ISettingsService
{
    private readonly IEventAggregator _eventAggregator;

    public SettingsService(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public ApplicationSettings GetApplicationSettings()
    {
        var billablePercentage = double.TryParse(
            ConfigurationManager.AppSettings["BillablePercentage"],
            out double billablePercentageResult
        )
            ? billablePercentageResult
            : 84d;
        var showOnlyWorkingDays = bool.TryParse(
            ConfigurationManager.AppSettings["ShowOnlyWorkingDays"],
            out bool result
        )
            ? result
            : true;

        return new ApplicationSettings(billablePercentage, showOnlyWorkingDays);
    }

    public void UpdateApplicationSettings(ApplicationSettings settings)
    {
        foreach (var setting in settings.AllSettings)
        {
            UpdateAppSettings(setting.Key, setting.Value);
        }

        _eventAggregator
            .GetEvent<ApplicationSettingsUpdatedEvent>()
            .Publish(new ApplicationSettingsUpdated(GetApplicationSettings()));
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
