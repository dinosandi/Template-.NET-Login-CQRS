using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PartComponent> Components { get; set; }
        public DbSet<ComponentActivity> ComponentActivities { get; set; }
        public DbSet<ComponentHistory> ComponentHistories { get; private set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<APL> APLs { get; set; }

        public DbSet<APLPart> APLParts { get; set; }
        public DbSet<PartComponentAPL> PartComponentAPLs { get; set; }
        public DbSet<ComponentCustomPart> ComponentCustomParts { get; set; }
        public DbSet<Historical> Historicals { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ComponentLifetime> ComponentLifetimes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Relasi Many-to-Many manual: PartComponent ↔ APL lewat PartComponentAPL
            modelBuilder.Entity<PartComponentAPL>()
                .HasKey(pca => new { pca.PartComponentId, pca.APLId });

            // Relasi ke PartComponent
            modelBuilder.Entity<PartComponentAPL>()
                .HasOne(pca => pca.PartComponent)
                .WithMany(pc => pc.PartComponentAPLs)
                .HasForeignKey(pca => pca.PartComponentId)
                .OnDelete(DeleteBehavior.Cascade);
            // ✅ Jika PartComponent dihapus → relasi PartComponentAPL ikut terhapus
            // ❌ Tapi APL dan APLPart tetap aman

            // Relasi ke APL
            modelBuilder.Entity<PartComponentAPL>()
                .HasOne(pca => pca.APL)
                .WithMany(apl => apl.PartComponentAPLs)
                .HasForeignKey(pca => pca.APLId)
                .OnDelete(DeleteBehavior.Restrict);
            // ✅ Jika APL dihapus → EF akan mencegah jika masih ada relasi aktif
            // (jaga integritas data)

            // 🔹 APL → APLPart (1-to-many)
            modelBuilder.Entity<APL>()
                .HasMany(a => a.Parts)
                .WithOne(p => p.APL)
                .HasForeignKey(p => p.APLId)
                .OnDelete(DeleteBehavior.Cascade);
            // ✅ Kalau APL dihapus, semua APLPart ikut terhapus

            // 🔹 Konfigurasi APL → PartComponent (optional relasi)
            modelBuilder.Entity<APL>()
                .HasOne(a => a.PartComponent)
                .WithMany(c => c.APLs)
                .HasForeignKey(a => a.PartComponentId)
                .OnDelete(DeleteBehavior.SetNull);
            // ✅ Kalau PartComponent dihapus, field FK di APL diset null, data tetap aman

            // 🔹 Konfigurasi User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.Role)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasIndex(e => e.Username).IsUnique();
            });

            // 🔹 ComponentActivity → PartComponent
            modelBuilder.Entity<ComponentActivity>()
                .HasOne(a => a.Component)
                .WithMany(c => c.ComponentActivities)
                .HasForeignKey(a => a.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);
            // ❌ Activity tidak ikut terhapus saat Component dihapus

            // 🔹 PartComponent → ComponentCustomPart (1-to-many)
            modelBuilder.Entity<ComponentCustomPart>()
                .HasOne(cp => cp.PartComponent)
                .WithMany(pc => pc.CustomParts)
                .HasForeignKey(cp => cp.PartComponentId)
                .OnDelete(DeleteBehavior.Cascade);
            // ✅ Kalau PartComponent dihapus → semua ComponentCustomPart ikut terhapus
            // 🔹 PartComponent → Historical (1-to-many)
            modelBuilder.Entity<Historical>()
                .HasOne(h => h.PartComponent)
                .WithMany(pc => pc.Historicals)
                .HasForeignKey(h => h.PartComponentId)
                .OnDelete(DeleteBehavior.Cascade);
            // Implementasi Token
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens) // add collection in User entity
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 PartComponent → Unit 
            modelBuilder.Entity<PartComponent>()
                .HasOne(p => p.Unit)
                .WithMany(u => u.PartComponents)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔹 PartComponent → ComponentLifetime (1-to-many)
            modelBuilder.Entity<ComponentLifetime>()
                .HasOne(cl => cl.PartComponent)
                .WithMany(pc => pc.ComponentLifetimes)
                .HasForeignKey(cl => cl.PartComponentId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
