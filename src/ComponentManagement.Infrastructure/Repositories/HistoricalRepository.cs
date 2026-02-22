using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class HistoricalRepository : IHistoricalRepository
    {
        private readonly AppDbContext _context;

        public HistoricalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Historical historical)
        {
            await _context.Historicals.AddAsync(historical);
        }

        public async Task UpdateAsync(Historical historical)
        {
            _context.Historicals.Update(historical);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Historical?> GetByIdAsync(Guid id)
        {
            return await _context.Historicals.FindAsync(id);
        }
    }
}
