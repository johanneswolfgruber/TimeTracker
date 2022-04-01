namespace TimeTracker.Contract.Notifications.Activities;

public sealed class ActivityCreatedOrUpdated : INotification
{
    public ActivityCreatedOrUpdated(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}
