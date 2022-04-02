namespace TimeTracker.WPF.ViewModels;

public class TrackingViewModel : BindableBase
{
    private readonly IMediator _mediator;
    private string _date = string.Empty;
    private string _startTime = string.Empty;
    private string? _endTime;
    private string _duration = string.Empty;

    public TrackingViewModel(IMediator mediator, TrackingDto tracking)
    {
        _mediator = mediator;

        StopTrackingCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);
        DeleteTrackingCommand = new DelegateCommand(async () => await OnDeleteTrackingAsync());

        Id = tracking.Id;
        Date = $"{DateOnly.FromDateTime(tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{tracking.StartTime.ToLocalTime():hh\\:mm}";
        EndTime = tracking.EndTime is null ? string.Empty : $"{tracking.EndTime.Value.ToLocalTime():hh\\:mm}";
        Duration = $"{tracking.Duration:hh\\:mm\\:ss}";
    }


    public Guid Id { get; }

    public DelegateCommand StopTrackingCommand { get; }
    
    public DelegateCommand DeleteTrackingCommand { get; }

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

    public void Update(TrackingDto tracking)
    {
        Date = $"{DateOnly.FromDateTime(tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{tracking.StartTime.ToLocalTime():hh\\:mm}";
        EndTime = tracking.EndTime is null ? string.Empty : $"{tracking.EndTime.Value.ToLocalTime():hh\\:mm}";
        Duration = $"{tracking.Duration:hh\\:mm\\:ss}";
    }

    private async Task OnStopTrackingAsync()
    {
        await _mediator.Send(new StopTrackingRequest(Id));
    }

    private async Task OnDeleteTrackingAsync()
    {
        await _mediator.Send(new DeleteTrackingRequest(Id));
    }

    private bool CanStopTracking() => string.IsNullOrEmpty(EndTime);
}

