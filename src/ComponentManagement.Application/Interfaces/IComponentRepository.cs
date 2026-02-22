using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IComponentRepository
    {
        Task AddAsync(PartComponent component);
        Task SaveChangesAsync();
        Task<PartComponent?> GetByIdAsync(Guid id);
        Task<IEnumerable<PartComponent>> GetAllAsync();
        Task UpdateAsync(PartComponent component);
        Task<PartComponent?> GetByIdWithHistoricalsAsync(Guid id);
        Task<PartComponent> GetByIdWithAplsAsync(Guid id);   
    }
}
