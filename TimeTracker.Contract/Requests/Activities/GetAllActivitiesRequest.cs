namespace TimeTracker.Contract.Requests.Activities;

public sealed class GetAllActivitiesRequest : IRequest<Result<GetAllActivitiesResponse>>
{
    public GetAllActivitiesRequest(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}

public sealed class GetAllActivitiesResponse : ApiResponse
{
    public GetAllActivitiesResponse(IEnumerable<ActivityDto> activities)
    {
        Activities = activities;
    }

    public IEnumerable<ActivityDto> Activities { get; }
}
