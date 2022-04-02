namespace TimeTracker.WPF.ViewModels;

public class CalendarOverviewViewModel : BindableBase, INavigationAware
{
    private readonly IMediator _mediator;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private Guid? _activityId;
    private TrackingViewModel? _selectedTracking;

    public CalendarOverviewViewModel(IMediator mediator, IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
    }
    
    public ObservableCollection<TrackingViewModel> Trackings { get; } = new();

    public TrackingViewModel? SelectedTracking
    {
        get => _selectedTracking;
        set
        {
            SetProperty(ref _selectedTracking, value);
            
            if (_selectedTracking is null)
            {
                return;
            }

            var parameters = new NavigationParameters();
            parameters.Add("ID", _selectedTracking.Id);
            _regionManager.RequestNavigate(RegionNames.CalendarDetailsRegion, nameof(CalendarDetailsView), parameters);
        }
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        _activityId = navigationContext.Parameters.GetValue<Guid>("ID");
        Initialize().Wait();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Unsubscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Unsubscribe(OnTrackingDeleted);
    }

    private async Task Initialize()
    {
        Trackings.Clear();

        if (_activityId is null)
        {
            return;
        }

        var response = await _mediator.Send(new GetAllTrackingsForActivityRequest(_activityId.Value));
        foreach (var tracking in response.Trackings)
        {
            AddOrUpdate(tracking);
        }
    }

    private void OnTrackingCreatedOrUpdated(TrackingCreatedOrUpdated notification)
    {
        if (notification.Tracking.ActivityId != _activityId)
        {
            return;
        }

        AddOrUpdate(notification.Tracking);
    }

    private void OnTrackingDeleted(TrackingDeleted notification)
    {
        var trackingToRemove = Trackings.FirstOrDefault(x => x.Id == notification.TrackingId);
        if (trackingToRemove is null)
        {
            return;
        }
        
        Trackings.Remove(trackingToRemove);
    }

    private TrackingViewModel Create(TrackingDto tracking)
    {
        return new TrackingViewModel(_mediator, tracking);
    }

    private void AddOrUpdate(TrackingDto tracking)
    {
        var existingTracking = Trackings.FirstOrDefault(x => x.Id == tracking.Id);
        if (existingTracking is null)
        {
            Trackings.Add(Create(tracking));
            return;
        }

        existingTracking.Update(tracking);
    }
}
