// AppDbContext.cs
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();

                // simpan enum sebagai string di database
                entity.Property(e => e.Role)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}
