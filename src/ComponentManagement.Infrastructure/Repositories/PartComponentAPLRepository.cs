using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class PartComponentAPLRepository : IPartComponentAPLRepository
    {
        private readonly AppDbContext _context;

        public PartComponentAPLRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PartComponentAPL?> GetByIdAsync(Guid id)
        {
            return await _context.PartComponentAPLs
                .Include(x => x.PartComponent)
                .Include(x => x.APL)
                .ThenInclude(a => a.Parts)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PartComponentAPL>> GetAllAsync()
        {
            return await _context.PartComponentAPLs
                .Include(x => x.PartComponent)
                .Include(x => x.APL)
                .ThenInclude(a => a.Parts)
                .ToListAsync();
        }

        public async Task<IEnumerable<PartComponentAPL>> GetByPartComponentIdAsync(Guid partComponentId)
        {
            return await _context.PartComponentAPLs
                .Include(x => x.APL)
                .ThenInclude(a => a.Parts)
                .Where(x => x.PartComponentId == partComponentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PartComponentAPL>> GetByAPLIdAsync(Guid aplId)
        {
            return await _context.PartComponentAPLs
                .Include(x => x.PartComponent)
                .Where(x => x.APLId == aplId)
                .ToListAsync();
        }

        public async Task AddAsync(PartComponentAPL entity)
        {
            await _context.PartComponentAPLs.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.PartComponentAPLs.FindAsync(id);
            if (entity != null)
            {
                _context.PartComponentAPLs.Remove(entity);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
