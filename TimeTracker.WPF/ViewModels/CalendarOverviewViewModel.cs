namespace TimeTracker.WPF.ViewModels;

public class CalendarOverviewViewModel : BindableBase, INavigationAware
{
    private readonly IMediator _mediator;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private Guid? _activityId;
    private TrackingViewModel? _selectedTracking;
    private ReadOnlyObservableCollection<object>? _selectedTrackings;
    private IDisposable? _timerDisposable;

    public CalendarOverviewViewModel(IMediator mediator, IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        
        ExportSelectedTrackingsCommand = new DelegateCommand<ReadOnlyObservableCollection<object>>(async items => await OnExportSelectedTrackings(items));
    }

    public DelegateCommand<ReadOnlyObservableCollection<object>> ExportSelectedTrackingsCommand { get; }

    public ObservableCollection<TrackingViewModel> Trackings { get; } = new();
    
    public Dictionary<string, TimeSpan> GroupDurations { get; } = new();

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
            parameters.Add("ID", _selectedTracking.Tracking.Id);
            _regionManager.RequestNavigate(RegionNames.CalendarDetailsRegion, nameof(CalendarDetailsView), parameters);
        }
    }

    public ReadOnlyObservableCollection<object>? SelectedTrackings
    {
        get => _selectedTrackings;
        set => SetProperty(ref _selectedTrackings, value);
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _eventAggregator.GetEvent<TrackingCreatedOrUpdatedEvent>().Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        _activityId = navigationContext.Parameters.GetValue<Guid>("ID");
        Initialize().Wait();
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
        StopTimer();
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

        SelectedTracking = Trackings.LastOrDefault();
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
        var trackingToRemove = Trackings.FirstOrDefault(x => x.Tracking.Id == notification.TrackingId);
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
        var existingTracking = Trackings.FirstOrDefault(x => x.Tracking.Id == tracking.Id);
        if (existingTracking is null)
        {
            Trackings.Add(Create(tracking));
            return;
        }

        existingTracking.Update(tracking);
    }

    private async Task OnExportSelectedTrackings(ReadOnlyObservableCollection<object> items)
    {
        if (!items.Any())
        {
            return;
        }
        
        var saveFileDialog = new SaveFileDialog { Filter = "Excel files | *.xlsx" };
        if (saveFileDialog.ShowDialog() == false)
        {
            return;
        }

        var directory = new FileInfo(saveFileDialog.FileName).Directory?.FullName;
        if (directory is null)
        {
            return;
        }
        
        var fileName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
        
        _ = await _mediator.Send(new ExportTrackingsRequest(
            Path.Combine(directory, fileName),
            items.Cast<TrackingViewModel>().Select(x => x.Tracking.Id)));
    }
    
    private void StartTimer()
    {
        _timerDisposable = Observable
            .Interval(TimeSpan.FromSeconds(1), DispatcherScheduler.Current)
            .StartWith(-1)
            .Subscribe(OnTimerTick);
    }

    private void StopTimer()
    {
        if (_timerDisposable is null)
        {
            return;
        }

        _timerDisposable.Dispose();
        _timerDisposable = null;
    }

    private void OnTimerTick(long i = -1)
    {
        var groups = Trackings.GroupBy(x => x.CalendarWeek);
        foreach (var group in groups)
        {
            var totalDuration = group.Select(x =>
            {
                if (string.IsNullOrEmpty(x.EndTime))
                {
                    var duration = DateTime.UtcNow - x.Tracking.StartTime;
                    x.UpdateDuration(duration);
                    _eventAggregator.GetEvent<DurationUpdatedEvent>().Publish(new DurationUpdated(x.Tracking.Id, duration));
                }

                return x.DurationTimeSpan;
            }).Sum();
            
            var groupId = group.Key;
            GroupDurations[groupId] = totalDuration;
        }
        
        RaisePropertyChanged(nameof(GroupDurations));
    }
}
