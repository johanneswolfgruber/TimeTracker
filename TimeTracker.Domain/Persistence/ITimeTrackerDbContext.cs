namespace TimeTracker.Domain.Persistence;

public interface ITimeTrackerDbContext : IDisposable
{
    DbSet<Activity> Activities { get; set; }

    DbSet<Tracking> Trackings { get; set; }

    string DbPath { get; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
