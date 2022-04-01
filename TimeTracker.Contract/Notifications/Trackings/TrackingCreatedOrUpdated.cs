namespace TimeTracker.Contract.Notifications.Trackings;

public sealed class TrackingCreatedOrUpdated : INotification
{
    public TrackingCreatedOrUpdated(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
