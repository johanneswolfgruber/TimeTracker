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
        IMapper mapper)
    {
        _contextFactory = contextFactory;
        _mediator = mediator;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<StartTrackingResponse> Handle(StartTrackingRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var activity = await context.Activities.FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);
        if (activity is null)
        {
            throw new InvalidOperationException("Cannot start tracking without an activity");
        }

        var tracking = new Tracking
        {
            Id = Guid.NewGuid(),
            ActivityId = request.ActivityId,
            Activity = activity,
            StartTime = _dateTimeProvider.UtcNow
        };

        activity.Trackings.Add(tracking);
        context.Trackings.Add(tracking);

        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return new StartTrackingResponse(dto);
    }

    public async Task<StopTrackingResponse> Handle(StopTrackingRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            throw new InvalidOperationException("Tracking not found");
        }

        if (tracking.EndTime.HasValue)
        {
            throw new InvalidOperationException("Tracking was already stopped");
        }

        tracking.EndTime = _dateTimeProvider.UtcNow;

        context.Entry(tracking).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return new StopTrackingResponse(dto);
    }

    public async Task<DeleteTrackingResponse> Handle(DeleteTrackingRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            return new DeleteTrackingResponse(false);
        }

        var entry = context.Trackings.Remove(tracking);

        await context.SaveChangesAsync(cancellationToken);

        await _mediator.Publish(new TrackingDeleted(request.TrackingId), cancellationToken);

        return new DeleteTrackingResponse(entry is not null);
    }

    public async Task<GetTrackingResponse> Handle(GetTrackingRequest request, CancellationToken cancellationToken)
    {
        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        return new GetTrackingResponse(tracking is null ? null : _mapper.Map<TrackingDto>(tracking));
    }

    public async Task<GetAllTrackingsForActivityResponse> Handle(GetAllTrackingsForActivityRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var trackings = await context.Trackings
            .Where(x => x.ActivityId == request.ActivityId)
            .Select(x => _mapper.Map<TrackingDto>(x))
            .ToListAsync(cancellationToken);

        return new GetAllTrackingsForActivityResponse(trackings);
    }

    public async Task<UpdateTrackingStartTimeResponse> Handle(UpdateTrackingStartTimeRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            throw new InvalidOperationException("Tracking not found");
        }

        tracking.StartTime = request.StartTime.ToUniversalTime();

        context.Entry(tracking).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return new UpdateTrackingStartTimeResponse(dto);
    }

    public async Task<UpdateTrackingEndTimeResponse> Handle(UpdateTrackingEndTimeRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            throw new InvalidOperationException("Tracking not found");
        }

        tracking.EndTime = request.EndTime.ToUniversalTime();

        context.Entry(tracking).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return new UpdateTrackingEndTimeResponse(dto);
    }

    public async Task<UpdateTrackingNotesResponse> Handle(UpdateTrackingNotesRequest request, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();

        var tracking = await GetTrackingAsync(request.TrackingId, cancellationToken);
        if (tracking is null)
        {
            throw new InvalidOperationException("Tracking not found");
        }

        tracking.Notes = request.Notes;

        context.Entry(tracking).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TrackingDto>(tracking);

        await _mediator.Publish(new TrackingCreatedOrUpdated(dto), cancellationToken);

        return new UpdateTrackingNotesResponse(dto);
    }

    private async Task<Tracking?> GetTrackingAsync(Guid trackingId, CancellationToken cancellationToken)
    {
        using var context = _contextFactory.Create();
        return await context.Trackings.FirstOrDefaultAsync(x => x.Id == trackingId, cancellationToken);
    }
}
