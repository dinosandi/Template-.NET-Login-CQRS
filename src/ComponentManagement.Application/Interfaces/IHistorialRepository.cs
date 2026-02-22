using System;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IHistoricalRepository
    {
        Task<Historical> GetByIdAsync(Guid id);
        Task AddAsync(Historical historical);
        Task UpdateAsync(Historical historical);
        Task SaveChangesAsync();
    }
}
