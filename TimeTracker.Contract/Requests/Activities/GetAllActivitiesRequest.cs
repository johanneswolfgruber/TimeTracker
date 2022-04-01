namespace TimeTracker.Contract.Requests.Activities;

public sealed class GetAllActivitiesRequest : IRequest<GetAllActivitiesResponse>
{
}

public sealed class GetAllActivitiesResponse
{
    public GetAllActivitiesResponse(IEnumerable<ActivityDto> activities)
    {
        Activities = activities;
    }

    public IEnumerable<ActivityDto> Activities { get; }
}