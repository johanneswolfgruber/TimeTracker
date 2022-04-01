namespace TimeTracker.Domain.Persistence;

public interface ITimeTrackerDbContextFactory
{
    ITimeTrackerDbContext Create();
}
