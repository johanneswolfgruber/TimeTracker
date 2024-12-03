namespace TimeTracker.WPF.ViewModels;

public class CalendarDetailsViewModel : BindableBase, INavigationAware
{
    private readonly IMediator _mediator;
    private readonly IEventAggregator _eventAggregator;
    private TrackingDto? _tracking;
    private DateTime? _startDate;
    private DateTime? _startTime;
    private DateTime? _endDate;
    private DateTime? _endTime;
    private string _duration = string.Empty;
    private string? _notes = string.Empty;
    private bool _isUserUpdate;

    public CalendarDetailsViewModel(IMediator mediator, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _eventAggregator = eventAggregator;

        StopTrackingCommand = new DelegateCommand(
            async () => await OnStopTrackingAsync(),
            CanStopTracking
        );
    }

    public DelegateCommand StopTrackingCommand { get; }

    public DateTime? StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    public DateTime? StartTime
    {
        get => _startTime;
        set => SetProperty(ref _startTime, value);
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
    }

    public DateTime? EndTime
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
        private set => SetProperty(ref _duration, value);
    }

    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _eventAggregator
            .GetEvent<TrackingCreatedOrUpdatedEvent>()
            .Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        _eventAggregator.GetEvent<DurationUpdatedEvent>().Subscribe(OnDurationUpdated);

        var trackingId = navigationContext.Parameters.GetValue<Guid>("ID");

        Initialize(trackingId).Wait();

        PropertyChanged += OnPropertyChanged;
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _eventAggregator
            .GetEvent<TrackingCreatedOrUpdatedEvent>()
            .Unsubscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Unsubscribe(OnTrackingDeleted);
        _eventAggregator.GetEvent<DurationUpdatedEvent>().Unsubscribe(OnDurationUpdated);

        PropertyChanged -= OnPropertyChanged;
    }

    private async Task Initialize(Guid? trackingId)
    {
        if (trackingId is null)
        {
            return;
        }

        var response = await _mediator.Send(new GetTrackingRequest(trackingId.Value));
        if (response.Failure)
        {
            return;
        }

        Update(response.Value.Tracking);
    }

    private void OnTrackingCreatedOrUpdated(TrackingCreatedOrUpdated notification)
    {
        if (notification.Tracking.Id != _tracking?.Id)
        {
            return;
        }

        Update(notification.Tracking);
    }

    private void OnTrackingDeleted(TrackingDeleted notification)
    {
        if (notification.TrackingId != _tracking?.Id)
        {
            return;
        }

        _tracking = null;
        StartDate = null;
        StartTime = null;
        EndDate = null;
        EndTime = null;
        Duration = string.Empty;
        Notes = string.Empty;
    }

    private async Task OnStopTrackingAsync()
    {
        if (_tracking is null)
        {
            return;
        }

        await _mediator.Send(new StopTrackingRequest(_tracking.Id));
    }

    private bool CanStopTracking() => EndTime is null;

    private void Update(TrackingDto tracking)
    {
        _tracking = tracking;

        Duration = tracking.EndTime is null
            ? (DateTime.UtcNow - _tracking.StartTime).ToDurationFormatStringFull()
            : tracking.Duration.ToDurationFormatStringFull();

        if (_isUserUpdate)
        {
            return;
        }

        StartDate = tracking.StartTime.ToLocalTime();
        StartTime = tracking.StartTime.ToLocalTime();
        EndDate = tracking.EndTime?.ToLocalTime();
        EndTime = tracking.EndTime?.ToLocalTime();
        Notes = tracking.Notes;
    }

    private void OnDurationUpdated(DurationUpdated args)
    {
        if (_tracking is null || _tracking.Id != args.TrackingId)
        {
            return;
        }

        Duration = args.Duration.ToDurationFormatStringFull();
    }

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(StartDate):
            case nameof(StartTime):
                await OnStartChanged();
                return;
            case nameof(EndDate):
            case nameof(EndTime):
                await OnEndChanged();
                return;
            case nameof(Notes):
                await OnNotesChanged();
                return;
            default:
                return;
        }
    }

    private async Task OnStartChanged()
    {
        if (StartDate is null || StartTime is null || _tracking is null)
        {
            return;
        }

        _isUserUpdate = true;
        var newStartTime = StartDate.Value.Date + StartTime.Value.TimeOfDay;
        var response = await _mediator.Send(
            new UpdateTrackingStartTimeRequest(_tracking.Id, newStartTime)
        );
        if (response.Failure)
        {
            return;
        }
        
        Update(response.Value.Tracking);
        _isUserUpdate = false;
    }

    private async Task OnEndChanged()
    {
        if (EndDate is null || EndTime is null || _tracking is null)
        {
            return;
        }

        _isUserUpdate = true;
        var newEndTime = EndDate.Value.Date + EndTime.Value.TimeOfDay;
        var response = await _mediator.Send(
            new UpdateTrackingEndTimeRequest(_tracking.Id, newEndTime)
        );
        if (response.Failure)
        {
            return;
        }
        
        Update(response.Value.Tracking);
        _isUserUpdate = false;
    }

    private async Task OnNotesChanged()
    {
        if (Notes is null || _tracking is null)
        {
            return;
        }

        _isUserUpdate = true;
        var response = await _mediator.Send(new UpdateTrackingNotesRequest(_tracking.Id, Notes));
        if (response.Failure)
        {
            return;
        }
        
        Update(response.Value.Tracking);
        _isUserUpdate = false;
    }
}
