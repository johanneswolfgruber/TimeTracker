namespace TimeTracker.Contract.Requests.Activities;

public sealed class CreateActivityRequest : IRequest<CreateActivityResponse>
{
    public CreateActivityRequest(string name)
    {
        Name = name;
    }

    public string Name { get; }
}

public sealed class CreateActivityResponse
{
    public CreateActivityResponse(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}