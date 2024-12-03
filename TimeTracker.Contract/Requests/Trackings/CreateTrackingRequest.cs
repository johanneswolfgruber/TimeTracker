namespace TimeTracker.Contract.Requests.Trackings;

public sealed class CreateTrackingRequest : IRequest<Result<CreateTrackingResponse>>
{
    public CreateTrackingRequest(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}

public sealed class CreateTrackingResponse : ApiResponse
{
    public CreateTrackingResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
