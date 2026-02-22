using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class APLPartRepository : IAPLPartRepository
    {
        private readonly AppDbContext _context;

        public APLPartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(APLPart aplPart)
        {
            await _context.APLParts.AddAsync(aplPart);
        }

        public async Task<IEnumerable<APLPart>> GetAllAsync()
        {
            return await _context.APLParts.ToListAsync();
        }

        public async Task<APLPart?> GetByIdAsync(Guid id)
        {
            return await _context.APLParts.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
