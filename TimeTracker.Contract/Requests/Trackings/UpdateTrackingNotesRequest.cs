namespace TimeTracker.Contract.Requests.Trackings;

public sealed class UpdateTrackingNotesRequest : IRequest<Result<UpdateTrackingNotesResponse>>
{
    public UpdateTrackingNotesRequest(Guid trackingId, string notes)
    {
        TrackingId = trackingId;
        Notes = notes;
    }

    public Guid TrackingId { get; }
    public string Notes { get; }
}

public sealed class UpdateTrackingNotesResponse
{
    public UpdateTrackingNotesResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
