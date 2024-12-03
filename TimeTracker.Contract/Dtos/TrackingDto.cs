namespace TimeTracker.Contract.Dtos;

public class TrackingDto(
    Guid id,
    string notes,
    DateTime startTime,
    DateTime? endTime,
    TimeSpan duration,
    Guid activityId
)
{
    public Guid Id { get; init; } = id;

    public string Notes { get; init; } = notes;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StartTime { get; init; } = startTime;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? EndTime { get; init; } = endTime;

    public TimeSpan Duration { get; init; } = duration;

    public Guid ActivityId { get; init; } = activityId;
}
