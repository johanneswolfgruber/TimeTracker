namespace TimeTracker.Contract.Requests.Activities;

public sealed class CreateActivityRequest : IRequest<Result<CreateActivityResponse>>
{
    public CreateActivityRequest(string name, Guid userId)
    {
        Name = name;
        UserId = userId;
    }

    public string Name { get; }

    public Guid UserId { get; }
}

public sealed class CreateActivityResponse : ApiResponse
{
    public CreateActivityResponse(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}
