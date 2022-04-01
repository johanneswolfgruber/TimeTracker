namespace TimeTracker.Domain.Model;

public class Tracking
{
    public Guid Id { get; set; }
    
    public string Notes { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;

    public Guid ActivityId { get; set; }

    public Activity Activity { get; set; } = default!;
}
