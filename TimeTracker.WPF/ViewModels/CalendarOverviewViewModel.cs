using System.Threading;

namespace TimeTracker.WPF.ViewModels;

public class CalendarOverviewViewModel : 
    BindableBase, 
    INavigationAware, 
    INotificationHandler<TrackingCreatedOrUpdated>
{
    private readonly IMediator _mediator;
    private readonly IRegionManager _regionManager;
    private Guid? _activityId = null;

    public CalendarOverviewViewModel(IMediator mediator, IRegionManager regionManager)
    {
        _mediator = mediator;
        _regionManager = regionManager;
    }

    public ObservableCollection<TrackingViewModel> Trackings { get; } = new();

    public Task Handle(TrackingCreatedOrUpdated notification, CancellationToken cancellationToken)
    {
        if (notification.Tracking.ActivityId != _activityId)
        {
            return Task.CompletedTask;
        }

        Trackings.Add(Create(notification.Tracking));
        return Task.CompletedTask;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _activityId = navigationContext.Parameters.GetValue<Guid>("ID");
        Initialize().Wait();
    }

    private async Task Initialize()
    {
        if (_activityId is null)
        {
            return;
        }

        var response = await _mediator.Send(new GetAllTrackingsForActivityRequest(_activityId.Value));
        Trackings.AddRange(response.Trackings.Select(Create));
    }

    private TrackingViewModel Create(TrackingDto tracking)
    {
        return new TrackingViewModel(tracking, OnStopTrackingAsync);
    }

    private async Task<TrackingDto> OnStopTrackingAsync(Guid id)
    {
        var response = await _mediator.Send(new StopTrackingRequest(id));
        return response.Tracking;
    }
}
