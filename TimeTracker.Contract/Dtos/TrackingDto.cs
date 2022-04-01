namespace TimeTracker.Contract.Dtos;

public class TrackingDto
{
    public TrackingDto(Guid id, string notes, DateTime startTime, DateTime? endTime, TimeSpan duration, Guid activityId)
    {
        Id = id;
        Notes = notes;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
        ActivityId = activityId;
    }

    public Guid Id { get; }

    public string Notes { get; }

    public DateTime StartTime { get; }

    public DateTime? EndTime { get; }

    public TimeSpan Duration { get; }

    public Guid ActivityId { get; }
}
