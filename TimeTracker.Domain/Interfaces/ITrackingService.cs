namespace TimeTracker.Domain;

public interface ITrackingService
    : IRequestHandler<CreateTrackingRequest, Result<CreateTrackingResponse>>,
        IRequestHandler<DeleteTrackingRequest, Result<DeleteTrackingResponse>>,
        IRequestHandler<GetTrackingRequest, Result<GetTrackingResponse>>,
        IRequestHandler<GetAllTrackingsRequest, Result<GetAllTrackingsResponse>>,
        IRequestHandler<UpdateTrackingRequest, Result<UpdateTrackingResponse>> { }
