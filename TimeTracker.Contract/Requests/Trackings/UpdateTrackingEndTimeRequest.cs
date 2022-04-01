namespace TimeTracker.Contract.Requests.Trackings;

public sealed class UpdateTrackingEndTimeRequest : IRequest<UpdateTrackingEndTimeResponse>
{
    public UpdateTrackingEndTimeRequest(Guid trackingId, DateTime endTime)
    {
        TrackingId = trackingId;
        EndTime = endTime;
    }

    public Guid TrackingId { get; }
    public DateTime EndTime { get; }
}

public sealed class UpdateTrackingEndTimeResponse
{
    public UpdateTrackingEndTimeResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}