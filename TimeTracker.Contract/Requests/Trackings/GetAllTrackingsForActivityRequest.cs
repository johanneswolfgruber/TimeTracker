namespace TimeTracker.Contract.Requests.Trackings;

public sealed class GetAllTrackingsForActivityRequest : IRequest<GetAllTrackingsForActivityResponse>
{
    public GetAllTrackingsForActivityRequest(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}

public sealed class GetAllTrackingsForActivityResponse
{
    public GetAllTrackingsForActivityResponse(IEnumerable<TrackingDto> trackings)
    {
        Trackings = trackings;
    }

    public IEnumerable<TrackingDto> Trackings { get; }
}