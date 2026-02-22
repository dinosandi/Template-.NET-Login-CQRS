using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
   public interface IUnitRepository
{
    IQueryable<Unit> GetQueryable();
    Task AddAsync(Unit unit);
    Task<IEnumerable<Unit>> GetAllAsync();
    Task<Unit?> GetByIdAsync(Guid id);
    void Update(Unit unit);
    void Delete(Unit unit);
    Task SaveChangesAsync();
}
}
