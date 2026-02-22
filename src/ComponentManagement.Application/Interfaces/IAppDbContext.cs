using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<PartComponent> Components { get; }
        DbSet<ComponentActivity> ComponentActivities { get; }
        DbSet<ComponentHistory> ComponentHistories { get; }
        DbSet<Part> Parts { get; }
        DbSet<APL> APLs { get; }
        DbSet<APLPart> APLParts { get; }
        DbSet<Unit> Units { get; }
        DbSet<ComponentLifetime> ComponentLifetimes { get; }
        DbSet<Notification> Notifications { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
