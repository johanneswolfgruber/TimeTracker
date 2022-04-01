namespace TimeTracker.Contract.Requests.Trackings;

public sealed class DeleteTrackingRequest : IRequest<DeleteTrackingResponse>
{
    public DeleteTrackingRequest(Guid trackingId)
    {
        TrackingId = trackingId;
    }

    public Guid TrackingId { get; }
}

public sealed class DeleteTrackingResponse
{
    public DeleteTrackingResponse(bool wasDeleted)
    {
        WasDeleted = wasDeleted;
    }

    public bool WasDeleted { get; }
}