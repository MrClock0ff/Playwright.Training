using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Playwright.Training.Domain.DAOs;

namespace Playwright.Training.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration: IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> builder)
    {
        builder
            .ToTable("users")
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique();
        
        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .Property(x => x.Id)
            .HasColumnName("id");
        
        builder
            .Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired();
        
        builder
            .Property(x => x.Username)
            .HasColumnName("username")
            .IsRequired();

        builder
            .Property(x => x.FirstName)
            .HasColumnName("first_name");
        
        builder
            .Property(x => x.LastName)
            .HasColumnName("last_name");
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("deleted");

        builder
            .HasData(new UserDao
            {
                Id = Guid.Parse("5c33e35c-606e-4d8f-91ec-1eba45094043"),
                Email = "daniel@training.com",
                Username = "daniel-training",
                FirstName = "Daniel",
                LastName = "Training",
                IsDeleted = false
            });
    }
}