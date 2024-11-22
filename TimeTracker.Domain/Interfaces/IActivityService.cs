namespace TimeTracker.Domain;

public interface IActivityService
    : IRequestHandler<CreateActivityRequest, Result<CreateActivityResponse>>,
        IRequestHandler<DeleteActivityRequest, Result<DeleteActivityResponse>>,
        IRequestHandler<GetActivityRequest, Result<GetActivityResponse>>,
        IRequestHandler<GetAllActivitiesRequest, Result<GetAllActivitiesResponse>>,
        IRequestHandler<UpdateActivityNameRequest, Result<UpdateActivityNameResponse>> { }
