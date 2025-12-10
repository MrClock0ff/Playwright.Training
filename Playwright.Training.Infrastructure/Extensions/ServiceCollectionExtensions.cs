using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Playwright.Training.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTrainingDatabaseContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TrainingDatabaseContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("Playwright.Training.Infrastructure");
                    sqlOptions.EnableRetryOnFailure(
                        10,
                        TimeSpan.FromSeconds(30),
                        null);
                });
        });

        return services;
    }
}