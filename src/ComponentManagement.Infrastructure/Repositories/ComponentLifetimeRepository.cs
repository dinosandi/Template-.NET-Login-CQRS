using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class ComponentLifetimeRepository : IComponentLifetimeRepository
    {
        private readonly AppDbContext _context;

        public ComponentLifetimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ComponentLifetime lifetime)
        {
            await _context.ComponentLifetimes.AddAsync(lifetime);
        }

        public async Task<ComponentLifetime?> GetActiveByComponentIdAsync(Guid partComponentId)
        {
            return await _context.ComponentLifetimes
                .Where(x =>
                    x.PartComponentId == partComponentId &&
                    x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
