namespace TimeTracker.Persistence;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigurePersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ITimeTrackerDbContextFactory, TimeTrackerDbContextFactory>();

        services.AddDbContext<TimeTrackerDbContext>();

        services
            .AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<TimeTrackerDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["AuthSettings:Audience"],
                    ValidIssuer = configuration["AuthSettings:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"]!)
                    ),
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddScoped(sp =>
        {
            var identityOptions = new IdentityOptions();
            var httpContext = sp.GetService<IHttpContextAccessor>()?.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated == true)
            {
                identityOptions.UserId = httpContext
                    .User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value;
                identityOptions.FirstName = httpContext.User.FindFirst(ClaimTypes.GivenName)!.Value;
                identityOptions.LastName = httpContext.User.FindFirst(ClaimTypes.Surname)!.Value;
            }
            return identityOptions;
        });

        return services.AddSingleton<ITimeTrackerDbContextFactory, TimeTrackerDbContextFactory>();
    }
}
