using System;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
public interface IAPLRepository
{
    Task AddAsync(APL apl);
    Task<IEnumerable<APL>> GetAllAsync();
    Task<APL?> GetByIdAsync(Guid id);
    Task<IQueryable<APL>> QueryAllAsync();
    void Delete(APLPart aplPart);
    void Update(APLPart aplPart);
    Task UpdateAsync(APL apl);
    Task DeleteAsync(APL apl);
    Task<APL?> GetByIdWithPartsAsync(Guid id); 
    Task SaveChangesAsync();

}

}
