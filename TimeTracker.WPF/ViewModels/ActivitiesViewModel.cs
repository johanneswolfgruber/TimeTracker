namespace TimeTracker.WPF.ViewModels;

public class ActivitiesViewModel : BindableBase
{
    private readonly IMediator _mediator;
    private readonly IRegionManager _regionManager;
    private string? _activityName;
    private ActivityViewModel? _selectedActivity;

    public ActivitiesViewModel(IMediator mediator, IRegionManager regionManager, IEventAggregator eventAggregator)
    {
        _mediator = mediator;
        _regionManager = regionManager;

        CreateActivityCommand = new DelegateCommand(async () => await OnCreateActivityAsync(), CanCreateActivity);

        eventAggregator.GetEvent<ActivityCreatedOrUpdatedEvent>().Subscribe(OnActivityCreatedOrUpdated);
        eventAggregator.GetEvent<ActivityDeletedEvent>().Subscribe(OnActivityDeleted);
        Initialize().Wait();
    }

    public DelegateCommand CreateActivityCommand { get; }

    public ObservableCollection<ActivityViewModel> Activities { get; } = new();

    public ActivityViewModel? SelectedActivity 
    { 
        get => _selectedActivity; 
        set
        {
            SetProperty(ref _selectedActivity, value);
            
            if (_selectedActivity is null)
            {
                return;
            }

            var parameters = new NavigationParameters();
            parameters.Add("ID", _selectedActivity.Id);
            _regionManager.RequestNavigate(RegionNames.CalendarOverviewRegion, nameof(CalendarOverviewView), parameters);
        }
    }

    public string? ActivityName
    {
        get => _activityName;
        set
        {
            SetProperty(ref _activityName, value);
            CreateActivityCommand.RaiseCanExecuteChanged();
        }
    }

    private async Task OnCreateActivityAsync()
    {
        if (string.IsNullOrEmpty(ActivityName))
        {
            throw new InvalidOperationException("Empty name is not allowed");
        }

        var response = await _mediator.Send(new CreateActivityRequest(ActivityName));
        Activities.Add(Create(response.Activity));
    }

    private bool CanCreateActivity() => !string.IsNullOrEmpty(ActivityName);

    private async Task Initialize()
    {
        Activities.Clear();
        var response = await _mediator.Send(new GetAllActivitiesRequest());
        Activities.AddRange(response.Activities.Select(Create));
        SelectedActivity = Activities.FirstOrDefault();
    }

    private ActivityViewModel Create(ActivityDto activity)
    {
        return new ActivityViewModel(_mediator, activity);
    }

    private void OnActivityCreatedOrUpdated(ActivityCreatedOrUpdated notification)
    {
    }

    private void OnActivityDeleted(ActivityDeleted notification)
    {
        var activityToRemove = Activities.FirstOrDefault(x => x.Id == notification.ActivityId);
        if (activityToRemove is null)
        {
            return;
        }
        
        Activities.Remove(activityToRemove);
        SelectedActivity = Activities.FirstOrDefault();
    }
}
