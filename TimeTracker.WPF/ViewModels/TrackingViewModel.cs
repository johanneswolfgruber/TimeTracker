namespace TimeTracker.WPF.ViewModels;

public class TrackingViewModel : BindableBase
{
    private readonly Func<Guid, Task<TrackingDto>> _stopTrackingCallback;
    private string _date = string.Empty;
    private string _startTime = string.Empty;
    private string? _endTime;
    private string _duration = string.Empty;

    public TrackingViewModel(TrackingDto tracking, Func<Guid, Task<TrackingDto>> stopTrackingCallback)
    {
        _stopTrackingCallback = stopTrackingCallback;

        StopTrackingCommand = new DelegateCommand(async () => await OnStopTrackingAsync(), CanStopTracking);

        Id = tracking.Id;
        Date = $"{DateOnly.FromDateTime(tracking.StartTime.ToLocalTime()):dd\\.MM\\.yy}";
        StartTime = $"{tracking.StartTime.ToLocalTime():hh\\:mm}";
        EndTime = tracking.EndTime is null ? string.Empty : $"{tracking.EndTime.Value.ToLocalTime():hh\\:mm}";
        Duration = $"{tracking.Duration:hh\\:mm\\:ss}";
    }


    public Guid Id { get; }

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

    private async Task OnStopTrackingAsync()
    {
        await _stopTrackingCallback(Id);
    }

    private bool CanStopTracking() => string.IsNullOrEmpty(EndTime);
}

