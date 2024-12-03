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

        PropertyChanged += OnPropertyChanged;
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

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Name):
                await OnNameChanged();
                return;
            default:
                return;
        }
    }

    private async Task OnNameChanged()
    {
        if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        _ = await _mediator.Send(new UpdateActivityNameRequest(Id, Name));
    }
}
