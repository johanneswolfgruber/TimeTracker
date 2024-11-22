namespace TimeTracker.Contract.Requests.Activities;

public sealed class DeleteActivityRequest : IRequest<Result<DeleteActivityResponse>>
{
    public DeleteActivityRequest(Guid activityId)
    {
        ActivityId = activityId;
    }

    public Guid ActivityId { get; }
}

public sealed class DeleteActivityResponse : ApiResponse
{
    public DeleteActivityResponse(bool wasDeleted)
    {
        WasDeleted = wasDeleted;
    }

    public bool WasDeleted { get; }
}
