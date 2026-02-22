using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IComponentActivityRepository
    {
        Task AddAsync(ComponentActivity activity);
        Task<ComponentActivity?> GetByIdAsync(Guid id);
        Task<IEnumerable<ComponentActivity>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
