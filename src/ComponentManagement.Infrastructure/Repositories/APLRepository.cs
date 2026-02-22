using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class APLRepository : IAPLRepository
    {
        private readonly AppDbContext _context;

        public APLRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(APL apl)
        {
            await _context.APLs.AddAsync(apl);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<APL?> GetByIdAsync(Guid id)
        {
            return await _context.APLs.FindAsync(id);
        }

        public async Task<IEnumerable<APL>> GetAllAsync()
        {
            return await _context.APLs.ToListAsync();
        }
        public async Task UpdateAsync(APL apl)
        {
            _context.APLs.Update(apl);
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(APL apl)
        {
            _context.APLs.Remove(apl);
            await Task.CompletedTask;
        }
        public async Task<APL?> GetByIdWithPartsAsync(Guid id)
        {
            return await _context.APLs
                .Include(a => a.Parts)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IQueryable<APL>> QueryAllAsync()
        {
            return await Task.FromResult(_context.APLs.AsQueryable());
        }
        public void Delete(APLPart aplPart)
        {
            _context.APLParts.Remove(aplPart);
        }
        public void Update(APLPart aplPart)
        {
            _context.APLParts.Update(aplPart);
        }
    }
}
