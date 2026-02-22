using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class ComponentCustomPartRepository : IComponentCustomPartRepository
    {
        private readonly AppDbContext _context;

        public ComponentCustomPartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComponentCustomPart>> GetAllAsync()
        {
            return await _context.ComponentCustomParts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ComponentCustomPart?> GetByIdAsync(Guid id)
        {
            return await _context.ComponentCustomParts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(ComponentCustomPart entity)
        {
            await _context.ComponentCustomParts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ComponentCustomPart entity)
        {
            _context.ComponentCustomParts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _context.ComponentCustomParts.FindAsync(id);
            if (existing != null)
            {
                _context.ComponentCustomParts.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
