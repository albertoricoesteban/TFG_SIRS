using SIRS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace SIRSWeb.StartupExtensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddCustomizedDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
       services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SIRSDBConnection"));

            // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            if (!env.IsProduction())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
        return services;
    }
}
