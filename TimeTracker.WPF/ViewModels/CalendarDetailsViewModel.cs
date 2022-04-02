namespace TimeTracker.WPF.ViewModels;

public class CalendarDetailsViewModel : BindableBase, INavigationAware
{
    private readonly IMediator _mediator;
    private readonly IEventAggregator _eventAggregator;
    private Guid? _trackingId;
    private string _date = string.Empty;
    private string _startTime = string.Empty;
    private string? _endTime;
    private string _duration = string.Empty;
    private string? _notes = string.Empty;

    public CalendarDetailsViewModel(IMediator mediator, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _eventAggregator = eventAggregator;
        
        StopTrackingCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);
    }
    
    public DelegateCommand StopTrackingCommand { get; }
    
    public string Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public string StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value);
    }

    public string? EndTime
    {
        get => _endTime;
        set
        {
            SetProperty(ref _endTime, value);
            StopTrackingCommand.RaiseCanExecuteChanged();
        }
    }

    public string Duration
    {
        get => _duration;
        set => SetProperty(ref _duration, value);
    }
    
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }
    
    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        _trackingId = navigationContext.Parameters.GetValue<Guid>("ID");
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
        if (_trackingId is null)
        {
            return;
        }

        var response = await _mediator.Send(new GetTrackingRequest(_trackingId.Value));
        if (response.Tracking is null)
        {
            return;
        }
        
        Date = $"{DateOnly.FromDateTime(response.Tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{response.Tracking.StartTime.ToLocalTime():hh\\:mm}";
        EndTime = response.Tracking.EndTime is null ? string.Empty : $"{response.Tracking.EndTime.Value.ToLocalTime():hh\\:mm}";
        Duration = $"{response.Tracking.Duration:hh\\:mm\\:ss}";
        Notes = response.Tracking.Notes;
    }
    
    private void OnTrackingCreatedOrUpdated(TrackingCreatedOrUpdated notification)
    {
        if (notification.Tracking.Id != _trackingId)
        {
            return;
        }

        Update(notification.Tracking);
    }

    private void OnTrackingDeleted(TrackingDeleted notification)
    {
        if (notification.TrackingId != _trackingId)
        {
            return;
        }

        Date = string.Empty;
        StartTime = string.Empty;
        EndTime = string.Empty;
        Duration = string.Empty;
    }
    
    private async Task OnStopTrackingAsync()
    {
        if (_trackingId is null)
        {
            return;
        }
        
        await _mediator.Send(new StopTrackingRequest(_trackingId.Value));
    }

    private bool CanStopTracking() => string.IsNullOrEmpty(EndTime);
    
    private void Update(TrackingDto tracking)
    {
        Date = $"{DateOnly.FromDateTime(tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{tracking.StartTime.ToLocalTime():hh\\:mm}";
        EndTime = tracking.EndTime is null ? string.Empty : $"{tracking.EndTime.Value.ToLocalTime():hh\\:mm}";
        Duration = $"{tracking.Duration:hh\\:mm\\:ss}";
        Notes = tracking.Notes;
    }
}