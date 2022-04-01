namespace TimeTracker.Domain;

public interface IActivityService : 
    IRequestHandler<CreateActivityRequest, CreateActivityResponse>,
    IRequestHandler<DeleteActivityRequest, DeleteActivityResponse>,
    IRequestHandler<GetActivityRequest, GetActivityResponse>,
    IRequestHandler<GetAllActivitiesRequest, GetAllActivitiesResponse>,
    IRequestHandler<UpdateActivityNameRequest, UpdateActivityNameResponse>
{
}
