namespace TimeTracker.Contract.Requests.Trackings;

public sealed class UpdateTrackingRequest : IRequest<Result<UpdateTrackingResponse>>
{
    public UpdateTrackingRequest(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}

public sealed class UpdateTrackingResponse : ApiResponse
{
    public UpdateTrackingResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
