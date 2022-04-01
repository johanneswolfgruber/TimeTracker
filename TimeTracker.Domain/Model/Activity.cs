namespace TimeTracker.Domain.Model;

public class Activity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();
}
