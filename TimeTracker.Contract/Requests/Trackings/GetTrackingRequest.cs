﻿namespace TimeTracker.Contract.Requests.Trackings;

public sealed class GetTrackingRequest : IRequest<Result<GetTrackingResponse>>
{
    public GetTrackingRequest(Guid trackingId)
    {
        TrackingId = trackingId;
    }

    public Guid TrackingId { get; }
}

public sealed class GetTrackingResponse : ApiResponse
{
    public GetTrackingResponse(TrackingDto tracking)
    {
        Tracking = tracking;
    }

    public TrackingDto Tracking { get; }
}
