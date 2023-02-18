namespace TimeTracker.WPF.Notifications.Events;

public class ApplicationSettingsUpdated
{
    public ApplicationSettingsUpdated(ApplicationSettings settings)
    {
        Settings = settings;
    }

    public ApplicationSettings Settings { get; }
}

public class ApplicationSettingsUpdatedEvent : PubSubEvent<ApplicationSettingsUpdated>
{
}