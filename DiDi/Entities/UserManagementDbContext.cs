using DiDi.Enums;
using Microsoft.EntityFrameworkCore;

namespace DiDi.Entities;

public sealed class UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.Email)
                .HasMaxLength(256)
                .IsRequired();

            entity.HasIndex(user => user.Email)
                .IsUnique();

            entity.Property(user => user.PasswordHash)
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.Gender)
                .HasConversion(
                    gender => gender == Gender.Male ? "男" : "女",
                    value => value == "男" || value == "Male" ? Gender.Male : Gender.Female)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(user => user.Province)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(user => user.City)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(user => user.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.HasIndex(user => new { user.Name, user.Age, user.Gender });
            entity.HasIndex(user => new { user.City, user.Gender });
        });
    }
}
