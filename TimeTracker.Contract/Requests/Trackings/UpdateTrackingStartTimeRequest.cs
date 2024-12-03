namespace TimeTracker.Contract.Requests.Trackings;

public sealed class UpdateTrackingStartTimeRequest
    : IRequest<Result<UpdateTrackingStartTimeResponse>>
{
    public UpdateTrackingStartTimeRequest(Guid trackingId, DateTime startTime)
    {
        TrackingId = trackingId;
        StartTime = startTime;
    }

    public Guid TrackingId { get; }
    public DateTime StartTime { get; }
}

public sealed class UpdateTrackingStartTimeResponse
{
    public UpdateTrackingStartTimeResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
