namespace TimeTracker.Contract.Requests.Activities;

public sealed class UpdateActivityNameRequest : IRequest<Result<UpdateActivityNameResponse>>
{
    public UpdateActivityNameRequest(Guid activityId, string name)
    {
        ActivityId = activityId;
        Name = name;
    }

    public Guid ActivityId { get; }
    public string Name { get; }
}

public sealed class UpdateActivityNameResponse : ApiResponse
{
    public UpdateActivityNameResponse(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}
