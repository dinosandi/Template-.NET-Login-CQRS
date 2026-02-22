using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IPartComponentAPLRepository
    {
        Task<PartComponentAPL?> GetByIdAsync(Guid id);
        Task<IEnumerable<PartComponentAPL>> GetAllAsync();
        Task<IEnumerable<PartComponentAPL>> GetByPartComponentIdAsync(Guid partComponentId);
        Task<IEnumerable<PartComponentAPL>> GetByAPLIdAsync(Guid aplId);
        Task AddAsync(PartComponentAPL entity);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
