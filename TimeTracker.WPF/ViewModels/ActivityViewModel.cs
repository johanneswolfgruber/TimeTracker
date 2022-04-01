namespace TimeTracker.WPF.ViewModels;

public class ActivityViewModel : BindableBase
{
    private readonly Func<Guid, Task<TrackingDto>> _startTrackingCallback;
    private string _name = string.Empty;

    public ActivityViewModel(ActivityDto activity, Func<Guid, Task<TrackingDto>> startTrackingCallback)
    {
        _startTrackingCallback = startTrackingCallback;

        Id = activity.Id;
        Name = activity.Name;

        StartTrackingCommand = new DelegateCommand(async () => await OnStartTrackingAsync());
    }

    public Guid Id { get; }

    public DelegateCommand StartTrackingCommand { get; }

    public ObservableCollection<TrackingDto> Trackings { get; } = new();

    public string Name 
    { 
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private async Task OnStartTrackingAsync()
    {
        var tracking = await _startTrackingCallback(Id);
        Trackings.Add(tracking);
    }
}
