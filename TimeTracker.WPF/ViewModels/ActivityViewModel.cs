namespace TimeTracker.WPF.ViewModels;

public class ActivityViewModel : BindableBase
{
    private readonly IMediator _mediator;
    private string _name = string.Empty;

    public ActivityViewModel(IMediator mediator, ActivityDto activity)
    {
        _mediator = mediator;

        Id = activity.Id;
        Name = activity.Name;

        StartTrackingCommand = new DelegateCommand(async () => await OnStartTrackingAsync());
        DeleteActivityCommand = new DelegateCommand(async () => await OnDeleteActivityAsync());
    }

    public Guid Id { get; }

    public DelegateCommand StartTrackingCommand { get; }
    
    public DelegateCommand DeleteActivityCommand { get; }

    public string Name 
    { 
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private async Task OnStartTrackingAsync()
    {
        _ = await _mediator.Send(new StartTrackingRequest(Id));
    }
    
    private async Task OnDeleteActivityAsync()
    {
        _ = await _mediator.Send(new DeleteActivityRequest(Id));
    }
}
