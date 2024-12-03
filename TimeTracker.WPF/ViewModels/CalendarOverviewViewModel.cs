namespace TimeTracker.WPF.ViewModels;

public class CalendarOverviewViewModel : BindableBase, INavigationAware
{
    private readonly IMediator _mediator;
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _eventAggregator;
    private readonly ISettingsService _settingsService;
    private Guid? _activityId;
    private string? _activityName;
    private double _billablePercentage;
    private bool _showOnlyWorkingDays;

    //private CalendarItemViewModel? _selectedCalendarItem;
    //private string? _selectedTotalTime;
    //private string? _billableTime;
    //private ReadOnlyObservableCollection<object>? _selectedTrackings;
    private IDisposable? _timerDisposable;
    private ObservableCollection<CalendarItemViewModel> _calendarItems = new();
    private ObservableCollection<CalendarItemViewModel> _selectedCalendarItems = new();

    public CalendarOverviewViewModel(
        IMediator mediator,
        IRegionManager regionManager,
        IEventAggregator eventAggregator,
        ISettingsService settingsService
    )
    {
        _mediator = mediator;
        _regionManager = regionManager;
        _eventAggregator = eventAggregator;
        _settingsService = settingsService;
        ExportSelectedTrackingsCommand = new DelegateCommand<ReadOnlyObservableCollection<object>>(
            async items => await OnExportSelectedTrackings(items)
        );
    }

    public DelegateCommand<
        ReadOnlyObservableCollection<object>
    > ExportSelectedTrackingsCommand { get; }

    public double BillablePercentage
    {
        get => _billablePercentage;
        set => SetProperty(ref _billablePercentage, value);
    }

    public bool ShowOnlyWorkingDays
    {
        get => _showOnlyWorkingDays;
        set => SetProperty(ref _showOnlyWorkingDays, value);
    }

    public ObservableCollection<CalendarItemViewModel> CalendarItems
    {
        get => _calendarItems;
        set => SetProperty(ref _calendarItems, value);
    }

    public ObservableCollection<CalendarItemViewModel> SelectedCalendarItems
    {
        get => _selectedCalendarItems;
        set
        {
            SetProperty(ref _selectedCalendarItems, value);

            if (_selectedCalendarItems.Count != 1)
            {
                var activeView = _regionManager
                    .Regions[RegionNames.CalendarDetailsRegion]
                    .ActiveViews.FirstOrDefault();

                if (activeView != null)
                {
                    _regionManager
                        .Regions[RegionNames.CalendarDetailsRegion]
                        .Deactivate(activeView);
                }

                return;
            }

            var parameters = new NavigationParameters
            {
                { "ID", _selectedCalendarItems[0].Tracking.Id },
            };
            _regionManager.RequestNavigate(
                RegionNames.CalendarDetailsRegion,
                nameof(CalendarDetailsView),
                parameters
            );
        }
    }

    //public Dictionary<string, TimeSpan> GroupDurations { get; } = new();

    //public CalendarItemViewModel? SelectedCalendarItem
    //{
    //    get => _selectedCalendarItem;
    //    set
    //    {
    //        SetProperty(ref _selectedCalendarItem, value);

    //        if (_selectedCalendarItem is null)
    //        {
    //            var activeView = _regionManager.Regions[RegionNames.CalendarDetailsRegion].ActiveViews.FirstOrDefault();

    //            if (activeView != null)
    //            {
    //                _regionManager.Regions[RegionNames.CalendarDetailsRegion].Deactivate(activeView);
    //            }

    //            return;
    //        }

    //        var parameters = new NavigationParameters
    //        {
    //            { "ID", _selectedCalendarItem.Tracking.Id }
    //        };
    //        _regionManager.RequestNavigate(RegionNames.CalendarDetailsRegion, nameof(CalendarDetailsView), parameters);
    //    }
    //}


    //public string? SelectedTotalTime
    //{
    //    get => _selectedTotalTime;
    //    set => SetProperty(ref _selectedTotalTime, value);
    //}

    //public string? BillableTime
    //{
    //    get => _billableTime;
    //    set => SetProperty(ref _billableTime, value);
    //}


    //public ReadOnlyObservableCollection<object>? SelectedTrackings
    //{
    //    get => _selectedTrackings;
    //    set
    //    {
    //        SetProperty(ref _selectedTrackings, value);
    //        SetTotalAndBillableTime();
    //    }
    //}

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _eventAggregator
            .GetEvent<TrackingCreatedOrUpdatedEvent>()
            .Subscribe(OnTrackingCreatedOrUpdated);
        _eventAggregator.GetEvent<TrackingDeletedEvent>().Subscribe(OnTrackingDeleted);
        _eventAggregator
            .GetEvent<ApplicationSettingsUpdatedEvent>()
            .Subscribe(OnApplicationSettingsUpdated);
        _activityId = navigationContext.Parameters.GetValue<Guid>("ID");
        _activityName = navigationContext.Parameters.GetValue<string>("ActivityName");
        Initialize().Wait();
        StartTimer();
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
        _eventAggregator
            .GetEvent<ApplicationSettingsUpdatedEvent>()
            .Unsubscribe(OnApplicationSettingsUpdated);
        StopTimer();
    }

    private async Task Initialize()
    {
        CalendarItems = new ObservableCollection<CalendarItemViewModel>();

        var settings = _settingsService.GetApplicationSettings();
        BillablePercentage = settings.BillablePercentage;
        ShowOnlyWorkingDays = settings.ShowOnlyWorkingDays;

        if (_activityId is null)
        {
            return;
        }

        var response = await _mediator.Send(
            new GetAllTrackingsForActivityRequest(_activityId.Value)
        );
        if (response.Failure)
        {
            return;
        }
        
        foreach (var tracking in response.Value.Trackings)
        {
            AddOrUpdate(tracking);
        }

        if (CalendarItems.Count != 0)
        {
            CalendarItems.Last().IsSelected = true; // todo: last from this week ?
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
        var trackingToRemove = CalendarItems.FirstOrDefault(x =>
            x.Tracking.Id == notification.TrackingId
        );
        if (trackingToRemove is null)
        {
            return;
        }

        CalendarItems.Remove(trackingToRemove);
    }

    private void OnApplicationSettingsUpdated(ApplicationSettingsUpdated notification)
    {
        ShowOnlyWorkingDays = notification.Settings.ShowOnlyWorkingDays;
    }

    private CalendarItemViewModel Create(TrackingDto tracking)
    {
        return new CalendarItemViewModel(_mediator, tracking, _activityName ?? string.Empty);
    }

    private void AddOrUpdate(TrackingDto tracking)
    {
        var existingCalendarItem = CalendarItems.FirstOrDefault(x => x.Tracking.Id == tracking.Id);
        if (existingCalendarItem is null)
        {
            CalendarItems.Add(Create(tracking));
            return;
        }

        existingCalendarItem.Update(tracking, _activityName ?? string.Empty);
    }

    private async Task OnExportSelectedTrackings(ReadOnlyObservableCollection<object> items)
    {
        if (!items.Any())
        {
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            FileName = "Leistungsnachweis",
            Filter = "Excel files | *.xlsx",
        };
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

        _ = await _mediator.Send(
            new ExportTrackingsRequest(
                Path.Combine(directory, fileName),
                items.Cast<TrackingViewModel>().Select(x => x.Tracking.Id)
            )
        );
    }

    private void StartTimer()
    {
        _timerDisposable = Observable
            .Interval(TimeSpan.FromSeconds(1)) // TODO: check how to run on dispatcher
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
        foreach (var item in CalendarItems)
        {
            if (item.Tracking.EndTime is not null)
            {
                continue;
            }

            item.UpdateDuration(DateTime.UtcNow - item.Tracking.StartTime);
        }

        //var groups = Trackings.GroupBy(x => x.CalendarWeek);
        //foreach (var group in groups)
        //{
        //    var totalDuration = group.Select(x =>
        //    {
        //        if (string.IsNullOrEmpty(x.EndTime))
        //        {
        //            var duration = DateTime.UtcNow - x.Tracking.StartTime;
        //            x.UpdateDuration(duration);
        //            _eventAggregator.GetEvent<DurationUpdatedEvent>().Publish(new DurationUpdated(x.Tracking.Id, duration));
        //        }

        //        return x.DurationTimeSpan;
        //    }).Sum();

        //    var groupId = group.Key;
        //    GroupDurations[groupId] = totalDuration;
        //}

        //RaisePropertyChanged(nameof(GroupDurations));
        //SetTotalAndBillableTime();
    }

    //private void SetTotalAndBillableTime()
    //{
    //    if (_selectedTrackings is null || _selectedTrackings.Count == 0)
    //    {
    //        SelectedTotalTime = null;
    //        BillableTime = null;
    //        return;
    //    }

    //    var setting = ConfigurationManager.AppSettings["BillablePercentage"] ?? "84";
    //    var factor = 0.84;

    //    if (double.TryParse(setting, out var billablePercentage))
    //    {
    //        factor = billablePercentage / 100;
    //    }

    //    var totalTime = _selectedTrackings.Cast<TrackingViewModel>().Select(x => x.DurationTimeSpan).Sum();
    //    SelectedTotalTime = totalTime.ToDurationFormatString();
    //    BillableTime = (totalTime * factor).ToDurationFormatString();
    //}
}
