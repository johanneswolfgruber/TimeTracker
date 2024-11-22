namespace TimeTracker.WPF.ViewModels;

public class CalendarItemViewModel : BindableBase
{
    private readonly IMediator _mediator;
    private string _activityName = string.Empty;
    private TimeSpan _durationTimeSpan;
    private bool _isSelected;

    public CalendarItemViewModel(IMediator mediator, TrackingDto tracking, string activityName)
    {
        _mediator = mediator;

        StopCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);
        DeleteCommand = new DelegateCommand(async () => await OnDeleteTrackingAsync());

        Update(tracking, activityName);
    }

    public TrackingDto Tracking { get; private set; } = default!;

    public DelegateCommand StopCommand { get; }

    public DelegateCommand DeleteCommand { get; }

    public string ActivityName
    {
        get => _activityName;
        set => SetProperty(ref _activityName, value);
    }

    public TimeSpan DurationTimeSpan
    {
        get => _durationTimeSpan;
        set => SetProperty(ref _durationTimeSpan, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public void Update(TrackingDto tracking, string activityName)
    {
        Tracking = tracking;
        ActivityName = activityName;
        DurationTimeSpan = Tracking.Duration;
    }

    public void UpdateDuration(TimeSpan duration)
    {
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

    private bool CanStopTracking() => Tracking.EndTime is null;
}
