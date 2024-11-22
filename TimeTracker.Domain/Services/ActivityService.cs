namespace TimeTracker.Domain;

public class ActivityService : IActivityService
{
    private readonly ITimeTrackerDbContextFactory _contextFactory;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ActivityService(
        ITimeTrackerDbContextFactory contextFactory,
        IMediator mediator,
        IMapper mapper
    )
    {
        _contextFactory = contextFactory;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Result<CreateActivityResponse>> Handle(
        CreateActivityRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Name,
        };

        context.Activities.Add(activity);
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ActivityDto>(activity);

        await _mediator.Publish(new ActivityCreatedOrUpdated(dto), cancellationToken);

        return Result.Ok(new CreateActivityResponse(dto));
    }

    public async Task<Result<DeleteActivityResponse>> Handle(
        DeleteActivityRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        if (activity is null)
        {
            return Result.Fail<DeleteActivityResponse>("Activity not found");
        }

        var entry = context.Activities.Remove(activity);
        await context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new ActivityDeleted(request.ActivityId), cancellationToken);

        return Result.Ok(new DeleteActivityResponse(entry is not null));
    }

    public async Task<Result<GetActivityResponse>> Handle(
        GetActivityRequest request,
        CancellationToken cancellationToken
    )
    {
        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        return activity is null
            ? Result.Fail<GetActivityResponse>("Activity not found")
            : Result.Ok(new GetActivityResponse(_mapper.Map<ActivityDto>(activity)));
    }

    public async Task<Result<GetAllActivitiesResponse>> Handle(
        GetAllActivitiesRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();
        var activities = await context
            .Activities.Where(x => x.UserId == request.UserId)
            .Include(x => x.Trackings)
            .Select(x => _mapper.Map<ActivityDto>(x))
            .ToListAsync(cancellationToken);

        return Result.Ok(new GetAllActivitiesResponse(activities));
    }

    public async Task<Result<UpdateActivityNameResponse>> Handle(
        UpdateActivityNameRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        if (activity is null)
        {
            return Result.Fail<UpdateActivityNameResponse>("Activity not found");
        }

        activity.Name = request.Name;

        context.Entry(activity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ActivityDto>(activity);

        await _mediator.Publish(new ActivityCreatedOrUpdated(dto), cancellationToken);

        return Result.Ok(new UpdateActivityNameResponse(dto));
    }

    private async Task<Activity?> GetActivityAsync(
        Guid activityId,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();
        return await context
            .Activities.Include(x => x.Trackings)
            .FirstOrDefaultAsync(x => x.Id == activityId, cancellationToken);
    }
}
