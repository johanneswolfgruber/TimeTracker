namespace TimeTracker.Persistence.Configurations;

public class TrackingConfiguration : IEntityTypeConfiguration<Tracking>
{
    public void Configure(EntityTypeBuilder<Tracking> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Activity).WithMany(x => x.Trackings).HasForeignKey(x => x.ActivityId);

        builder.Property(x => x.StartTime).IsRequired();

        builder.Property(x => x.EndTime);

        builder.Property(x => x.Notes);
    }
}
