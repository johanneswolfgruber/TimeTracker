namespace TimeTracker.Persistence;

public class TimeTrackerDbContext : IdentityDbContext<ApplicationUser>, ITimeTrackerDbContext
{
    public TimeTrackerDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Path.Combine(Environment.GetFolderPath(folder), "TimeTracker");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        DbPath = Path.Combine(path, "time_tracker.sqlite");
    }

    public DbSet<Activity> Activities { get; set; } = default!;

    public DbSet<Tracking> Trackings { get; set; } = default!;

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(TimeTrackerDbContext).Assembly);
    }
}
