namespace TimeTracker.WPF.ViewModels;

public class TrackingViewModel : BindableBase
{
    private readonly IMediator _mediator;
    private readonly Calendar _calendar;
    private string _date = string.Empty;
    private string _startTime = string.Empty;
    private string? _endTime;
    private string _duration = string.Empty;
    private TimeSpan _durationTimeSpan;
    private bool _isExpanded;

    public TrackingViewModel(IMediator mediator, TrackingDto tracking)
    {
        _mediator = mediator;
        _calendar = CultureInfo.CurrentCulture.Calendar;

        StopTrackingCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);
        DeleteTrackingCommand = new DelegateCommand(async () => await OnDeleteTrackingAsync());

        Update(tracking);
    }


    public TrackingDto Tracking { get; private set; } = default!;

    public DelegateCommand StopTrackingCommand { get; }
    
    public DelegateCommand DeleteTrackingCommand { get; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

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

    public TimeSpan DurationTimeSpan
    {
        get => _durationTimeSpan;
        set => SetProperty(ref _durationTimeSpan, value);
    }

    public string CalendarWeek => $"KW{_calendar.GetWeekOfYear(Tracking.StartTime.ToLocalTime(), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)}";

    public void Update(TrackingDto tracking)
    {
        Tracking = tracking;
        Date = $"{DateOnly.FromDateTime(tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{tracking.StartTime.ToLocalTime():HH\\:mm}";
        EndTime = tracking.EndTime is null ? string.Empty : $"{tracking.EndTime.Value.ToLocalTime():HH\\:mm}";
        Duration = tracking.Duration.ToDurationFormatString();
        DurationTimeSpan = tracking.Duration;
        RaisePropertyChanged(nameof(CalendarWeek));
    }

    public void UpdateDuration(TimeSpan duration)
    {
        Duration = duration.ToDurationFormatString();
        DurationTimeSpan = duration; 
    }

    private async Task OnStopTrackingAsync()
    {
        await _mediator.Send(new StopTrackingRequest(Tracking.Id));
    }

    private async Task OnDeleteTrackingAsync()
    {
        await _mediator.Send(new DeleteTrackingRequest(Tracking.Id));
    }

    private bool CanStopTracking() => string.IsNullOrEmpty(EndTime);
}

