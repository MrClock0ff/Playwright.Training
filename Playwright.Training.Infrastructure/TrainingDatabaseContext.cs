using Microsoft.EntityFrameworkCore;
using Playwright.Training.Domain.DAOs;

namespace Playwright.Training.Infrastructure;

public class TrainingDatabaseContext : DbContext
{
    public TrainingDatabaseContext(DbContextOptions<TrainingDatabaseContext> options) : base(options)
    {
    }

    public TrainingDatabaseContext()
    {
    }
    
    public DbSet<UserDao> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrainingDatabaseContext).Assembly);
    }
}