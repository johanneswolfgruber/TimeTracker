namespace TimeTracker.Contract.Requests.Activities;

public sealed class UpdateActivityNameRequest : IRequest<UpdateActivityNameResponse>
{
    public UpdateActivityNameRequest(Guid activityId, string name)
    {
        ActivityId = activityId;
        Name = name;
    }

    public Guid ActivityId { get; }
    public string Name { get; }
}

public sealed class UpdateActivityNameResponse
{
    public UpdateActivityNameResponse(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}