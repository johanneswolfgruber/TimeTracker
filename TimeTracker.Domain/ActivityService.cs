namespace TimeTracker.Domain;

public class ActivityService : IActivityService
{
    private static readonly Guid UserId = new("80F12913-17EA-43CA-9924-697BEFF3AFD5"); //TODO: until implementing user management
    private readonly ITimeTrackerDbContextFactory _contextFactory;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ActivityService(ITimeTrackerDbContextFactory contextFactory, IMediator mediator, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<CreateActivityResponse> Handle(CreateActivityRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            Name = request.Name
        };

        context.Activities.Add(activity);
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ActivityDto>(activity);

        await _mediator.Publish(new ActivityCreatedOrUpdated(dto), cancellationToken);

        return new CreateActivityResponse(dto);
    }

    public async Task<DeleteActivityResponse> Handle(DeleteActivityRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        if (activity is null)
        {
            return new DeleteActivityResponse(false);
        }

        var entry = context.Activities.Remove(activity);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteActivityResponse(entry is not null);
    }

    public async Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        return new GetActivityResponse(activity is null ? null : _mapper.Map<ActivityDto>(activity));
    }

    public async Task<GetAllActivitiesResponse> Handle(GetAllActivitiesRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();
        var activities = await context.Activities
            .Select(x => _mapper.Map<ActivityDto>(x))
            .ToListAsync(cancellationToken);

        return new GetAllActivitiesResponse(activities);
    }

    public async Task<UpdateActivityNameResponse> Handle(UpdateActivityNameRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var activity = await GetActivityAsync(request.ActivityId, cancellationToken);
        if (activity is null)
        {
            throw new InvalidOperationException("Activity not found");
        }

        activity.Name = request.Name;

        context.Entry(activity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ActivityDto>(activity);

        await _mediator.Publish(new ActivityCreatedOrUpdated(dto), cancellationToken);

        return new UpdateActivityNameResponse(dto);
    }

    private async Task<Activity?> GetActivityAsync(Guid activityId, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();
        return await context.Activities.FirstOrDefaultAsync(x => x.Id == activityId, cancellationToken);
    }
}
