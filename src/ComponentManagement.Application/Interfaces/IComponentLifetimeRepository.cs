using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IComponentLifetimeRepository
    {
        Task AddAsync(ComponentLifetime lifetime);
        Task<ComponentLifetime?> GetActiveByComponentIdAsync(Guid partComponentId);
        Task SaveChangesAsync();
    }
}
