namespace TimeTracker.Persistence;

public class TimeTrackerDbContext : DbContext, ITimeTrackerDbContext
{
    public TimeTrackerDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Path.Combine(Environment.GetFolderPath(folder), "TimeTracker");
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        DbPath = Path.Combine(path, "time_tracker.sqlite");
    }

    public DbSet<Activity> Activities { get; set; } = default!;
    
    public DbSet<Tracking> Trackings { get; set; } = default!;

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeTrackerDbContext).Assembly);
    }
}
