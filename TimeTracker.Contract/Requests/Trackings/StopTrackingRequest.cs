namespace TimeTracker.Contract.Requests.Trackings;

public sealed class StopTrackingRequest : IRequest<StopTrackingResponse>
{
    public StopTrackingRequest(Guid trackingId)
    {
        TrackingId = trackingId;
    }

    public Guid TrackingId { get; }
}

public sealed class StopTrackingResponse
{
    public StopTrackingResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}