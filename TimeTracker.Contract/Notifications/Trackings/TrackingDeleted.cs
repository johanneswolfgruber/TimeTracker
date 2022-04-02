namespace TimeTracker.Contract.Notifications.Trackings;

public class TrackingDeleted : INotification
{
    public TrackingDeleted(Guid trackingId)
    {
        TrackingId = trackingId;
    }

    public Guid TrackingId { get; }
}