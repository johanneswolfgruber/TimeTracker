namespace TimeTracker.Contract.Requests.Activities;

public sealed class GetActivityRequest : IRequest<Result<GetActivityResponse>>
{
    public GetActivityRequest(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}

public sealed class GetActivityResponse : ApiResponse
{
    public GetActivityResponse(ActivityDto activity)
    {
        Activity = activity;
    }

    public ActivityDto Activity { get; }
}
