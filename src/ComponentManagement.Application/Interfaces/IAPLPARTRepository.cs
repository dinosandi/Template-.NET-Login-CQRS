using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IAPLPartRepository
    {
        Task AddAsync(APLPart aplPart);
        Task<IEnumerable<APLPart>> GetAllAsync();
        Task<APLPart?> GetByIdAsync(Guid id);
        void Delete(APLPart aplPart);
        void Update(APLPart aplPart);
        Task SaveChangesAsync();
    }
}
