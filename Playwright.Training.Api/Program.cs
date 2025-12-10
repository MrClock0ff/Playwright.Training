using Microsoft.EntityFrameworkCore;
using Playwright.Training.Api;
using Playwright.Training.Infrastructure;

IHostBuilder builder = Host.CreateDefaultBuilder(args);
builder.ConfigureWebHostDefaults(configure =>
{
    configure.UseStartup<Startup>();
});

IHost app = builder.Build();

using (IServiceScope serviceScope = app.Services.CreateScope())
{
    TrainingDatabaseContext dbContext = serviceScope.ServiceProvider.GetRequiredService<TrainingDatabaseContext>();
    IEnumerable<string> pendingMigrations = dbContext.Database.GetPendingMigrations();

    if (pendingMigrations.Any())
    {
        dbContext.Database.Migrate();
    }
}

app.Run();