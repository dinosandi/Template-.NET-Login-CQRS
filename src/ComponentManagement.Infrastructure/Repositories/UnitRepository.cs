using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext _context;

        public UnitRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Unit> GetQueryable()
    {
        return _context.Units.AsQueryable();
    }

        public async Task AddAsync(Unit unit)
        {
            await _context.Units.AddAsync(unit);
        }


        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            return await _context.Units.ToListAsync();
        }

        public async Task<Unit?> GetByIdAsync(Guid id)
        {
            return await _context.Units.FindAsync(id);
        }

        public void Update(Unit unit)
        {
            _context.Units.Update(unit);
        }

        public void Delete(Unit unit)
        {
            _context.Units.Remove(unit);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
