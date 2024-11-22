namespace TimeTracker.Domain;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureBackendServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IActivityService>());
        services.AddAutoMapper(typeof(AutomapperProfile));

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
