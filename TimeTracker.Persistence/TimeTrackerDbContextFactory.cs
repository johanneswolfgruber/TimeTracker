namespace TimeTracker.Persistence;

public class TimeTrackerDbContextFactory : ITimeTrackerDbContextFactory
{
    public ITimeTrackerDbContext Create()
    {
        return new TimeTrackerDbContext();
    }
}
