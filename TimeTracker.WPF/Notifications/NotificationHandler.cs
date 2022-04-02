namespace TimeTracker.WPF.Notifications;

public class NotificationHandler :
    INotificationHandler<ActivityCreatedOrUpdated>,
    INotificationHandler<ActivityDeleted>,
    INotificationHandler<TrackingCreatedOrUpdated>,
    INotificationHandler<TrackingDeleted>
{
    private readonly IEventAggregator _eventAggregator;

    public NotificationHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public Task Handle(ActivityCreatedOrUpdated notification, CancellationToken cancellationToken)
    {
        _eventAggregator.GetEvent<ActivityCreatedOrUpdatedEvent>().Publish(notification);
        return Task.CompletedTask;
    }

    public Task Handle(ActivityDeleted notification, CancellationToken cancellationToken)
    {
        _eventAggregator.GetEvent<ActivityDeletedEvent>().Publish(notification);
        return Task.CompletedTask;
    }

    public Task Handle(TrackingCreatedOrUpdated notification, CancellationToken cancellationToken)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Publish(notification);
        return Task.CompletedTask;
    }

    public Task Handle(TrackingDeleted notification, CancellationToken cancellationToken)
    {
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Publish(notification);
        return Task.CompletedTask;
    }
}