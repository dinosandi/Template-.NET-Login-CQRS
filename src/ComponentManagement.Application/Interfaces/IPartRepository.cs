using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IPartRepository
    {
        Task AddAsync(Part part);
        Task<IEnumerable<Part>> GetAllAsync();
        Task<Part?> GetByIdAsync(Guid id);
        void Delete(Part part);
        Task SaveChangesAsync();
    }
}
