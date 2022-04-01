namespace TimeTracker.Contract.Dtos;

public class ActivityDto
{
    public ActivityDto(Guid id, Guid userId, string name, ICollection<TrackingDto> trackings)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Trackings = trackings;
    }

    public Guid Id { get; }

    public Guid UserId { get; }

    public string Name { get; }

    public ICollection<TrackingDto> Trackings { get; }
}
