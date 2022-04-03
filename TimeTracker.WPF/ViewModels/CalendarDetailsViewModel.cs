using System.Windows.Threading;

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
    private DispatcherTimer? _timer;
    private bool _isUserUpdate;

    public CalendarDetailsViewModel(IMediator mediator, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _eventAggregator = eventAggregator;
        
        StopTrackingCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);
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
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        
        var trackingId = navigationContext.Parameters.GetValue<Guid>("ID");
        
        Initialize(trackingId).Wait();

        PropertyChanged += OnPropertyChanged;

        StartTimer();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Unsubscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Unsubscribe(OnTrackingDeleted);
        
        PropertyChanged -= OnPropertyChanged;
        
        StopTimer();
    }

    private async Task Initialize(Guid? trackingId)
    {
        if (trackingId is null)
        {
            return;
        }

        var response = await _mediator.Send(new GetTrackingRequest(trackingId.Value));
        if (response.Tracking is null)
        {
            return;
        }

        Update(response.Tracking);
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
        StopTimer();
    }

    private bool CanStopTracking() => EndTime is null;

    private void Update(TrackingDto tracking)
    {
        _tracking = tracking;
        
        Duration = tracking.EndTime is null 
            ? $"{(int)(DateTime.UtcNow - _tracking.StartTime).TotalHours:d2}:" +
              $"{(DateTime.UtcNow - _tracking.StartTime).Minutes:d2}:" +
              $"{(DateTime.UtcNow - _tracking.StartTime).Seconds:d2}" 
            : $"{(int)tracking.Duration.TotalHours:d2}:" +
              $"{tracking.Duration.Minutes:d2}:" +
              $"{tracking.Duration.Seconds:d2}";
        
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

    private void StartTimer()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    private void StopTimer()
    {
        if (_timer is null)
        {
            return;
        }

        _timer.Tick -= OnTimerTick;
        _timer.Stop();
        _timer = null;
    }

    private void OnTimerTick(object? sender, EventArgs args)
    {
        if (EndTime is not null || _tracking is null)
        {
            return;
        }

        Duration = $"{(int)(DateTime.UtcNow - _tracking.StartTime).TotalHours:d2}:" +
                   $"{(DateTime.UtcNow - _tracking.StartTime).Minutes:d2}:" +
                   $"{(DateTime.UtcNow - _tracking.StartTime).Seconds:d2}";
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
        var response = await _mediator.Send(new UpdateTrackingStartTimeRequest(_tracking.Id, newStartTime));
        Update(response.Tracking);
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
        var response = await _mediator.Send(new UpdateTrackingEndTimeRequest(_tracking.Id, newEndTime));
        Update(response.Tracking);
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
        Update(response.Tracking);
        _isUserUpdate = false;
    }
}