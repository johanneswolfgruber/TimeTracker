namespace TimeTracker.Contract.Notifications.Activities;

public class ActivityDeleted : INotification
{
    public ActivityDeleted(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}