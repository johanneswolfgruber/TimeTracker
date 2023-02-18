namespace TimeTracker.WPF.Services;

public interface ISettingsService
{
    ApplicationSettings GetApplicationSettings();

    void UpdateApplicationSettings(ApplicationSettings settings);
}
