namespace TimeTracker.Contract.Requests.Trackings;

public sealed class DeleteTrackingRequest : IRequest<Result<DeleteTrackingResponse>>
{
    public DeleteTrackingRequest(Guid trackingId)
    {
        TrackingId = trackingId;
    }

    public Guid TrackingId { get; }
}

public sealed class DeleteTrackingResponse : ApiResponse
{
    public DeleteTrackingResponse(bool wasDeleted)
    {
        WasDeleted = wasDeleted;
    }

    public bool WasDeleted { get; }
}
