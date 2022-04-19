namespace TimeTracker.WPF.Notifications.Events;

public class DurationUpdated
{
    public DurationUpdated(Guid trackingId, TimeSpan duration)
    {
        TrackingId = trackingId;
        Duration = duration;
    }

    public Guid TrackingId { get; }
    
    public TimeSpan Duration { get; }
}

public class DurationUpdatedEvent : PubSubEvent<DurationUpdated>
{
}