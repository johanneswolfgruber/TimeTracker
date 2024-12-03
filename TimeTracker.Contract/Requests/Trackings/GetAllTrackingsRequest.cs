namespace TimeTracker.Contract.Requests.Trackings;

public sealed class GetAllTrackingsRequest : IRequest<Result<GetAllTrackingsResponse>> { }

public sealed class GetAllTrackingsResponse : ApiResponse
{
    public GetAllTrackingsResponse(IEnumerable<TrackingDto> trackings)
    {
        Trackings = trackings;
    }

    public IEnumerable<TrackingDto> Trackings { get; }
}
