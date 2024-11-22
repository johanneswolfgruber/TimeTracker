namespace TimeTracker.Contract.Requests.Trackings;

public sealed class StartTrackingRequest : IRequest<Result<StartTrackingResponse>>
{
    public StartTrackingRequest(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}

public sealed class StartTrackingResponse
{
    public StartTrackingResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
