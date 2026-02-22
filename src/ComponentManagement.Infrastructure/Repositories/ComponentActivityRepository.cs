using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Infrastructure.Repositories
{
    public class ComponentActivityRepository : IComponentActivityRepository
    {
        private readonly AppDbContext _context;

        public ComponentActivityRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(ComponentActivity activity)
        {
            await _context.ComponentActivities.AddAsync(activity);
        }

        public async Task<ComponentActivity?> GetByIdAsync(Guid id)
        {
            return await _context.ComponentActivities.FindAsync(id);
        }

        public async Task<IEnumerable<ComponentActivity>> GetAllAsync()
        {
            return await _context.ComponentActivities.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
