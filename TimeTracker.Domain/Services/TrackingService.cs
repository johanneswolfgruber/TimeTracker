namespace TimeTracker.Domain;

public class TrackingService : ITrackingService
{
    private readonly ITimeTrackerDbContextFactory _contextFactory;
    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public TrackingService(
        ITimeTrackerDbContextFactory contextFactory,
        IMediator mediator,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper
    )
    {
        _contextFactory = contextFactory;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<CreateTrackingResponse>> Handle(
        CreateTrackingRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var activity = await context.Activities.FirstOrDefaultAsync(
            x => x.Id == request.ActivityId,
            cancellationToken
        );
        if (activity is null)
        {
            return Result.Fail<CreateTrackingResponse>("Cannot start tracking without an activity");
        }

        var tracking = new Tracking
        {
            Id = Guid.NewGuid(),
            ActivityId = request.ActivityId,
            Activity = activity,
            StartTime = _dateTimeProvider.UtcNow,
        };

        activity.Trackings.Add(tracking);
        context.Trackings.Add(tracking);

        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return Result.Ok(new CreateTrackingResponse(dto));
    }

    public async Task<Result<UpdateTrackingResponse>> Handle(
        UpdateTrackingRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.Tracking.Id, cancellationToken);
        if (tracking is null)
        {
            return Result.Fail<UpdateTrackingResponse>("Tracking not found");
        }

        tracking.StartTime = request.Tracking.StartTime.ToUniversalTime();
        tracking.EndTime = request.Tracking.EndTime?.ToUniversalTime();
        tracking.Notes = request.Tracking.Notes;

        context.Entry(tracking).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return Result.Ok(new UpdateTrackingResponse(dto));
    }

    public async Task<Result<DeleteTrackingResponse>> Handle(
        DeleteTrackingRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            return Result.Fail<DeleteTrackingResponse>("Tracking not found");
        }

        var entry = context.Trackings.Remove(tracking);

        await context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new TrackingDeleted(request.TrackingId), cancellationToken);

        return Result.Ok(new DeleteTrackingResponse(entry is not null));
    }

    public async Task<Result<GetTrackingResponse>> Handle(
        GetTrackingRequest request,
        CancellationToken cancellationToken
    )
    {
        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        return tracking is null
            ? Result.Fail<GetTrackingResponse>("Tracking not found")
            : Result.Ok(new GetTrackingResponse(_mapper.Map<TrackingDto>(tracking)));
    }

    public async Task<Result<GetAllTrackingsResponse>> Handle(
        GetAllTrackingsRequest request,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();

        var trackings = await context
            .Trackings.Select(x => _mapper.Map<TrackingDto>(x))
            .ToListAsync(cancellationToken);

        return Result.Ok(new GetAllTrackingsResponse(trackings));
    }

    private async Task<Tracking?> GetTrackingAsync(
        Guid trackingId,
        CancellationToken cancellationToken
    )
    {
        using var context = _contextFactory.Create();
        return await context.Trackings.FirstOrDefaultAsync(
            x => x.Id == trackingId,
            cancellationToken
        );
    }
}
