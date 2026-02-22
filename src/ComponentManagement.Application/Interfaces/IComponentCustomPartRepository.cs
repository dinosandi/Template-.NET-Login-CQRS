using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IComponentCustomPartRepository
    {
        Task<IEnumerable<ComponentCustomPart>> GetAllAsync();
        Task<ComponentCustomPart?> GetByIdAsync(Guid id);
        Task AddAsync(ComponentCustomPart entity);
        Task UpdateAsync(ComponentCustomPart entity);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
