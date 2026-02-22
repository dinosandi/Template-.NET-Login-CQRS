using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Infrastructure.Persistence;


namespace ComponentManagement.Infrastructure.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly AppDbContext _context;

        public ComponentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PartComponent component)
        {
            await _context.Components.AddAsync(component);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PartComponent?> GetByIdAsync(Guid id)
        {
            return await _context.Components.FindAsync(id);
        }

        public async Task<IEnumerable<PartComponent>> GetAllAsync()
        {
            return await _context.Components.ToListAsync();
        }
        public async Task UpdateAsync(PartComponent component)
        {
            _context.Components.Update(component);
            await Task.CompletedTask;
        }
        public async Task<PartComponent?> GetByIdWithHistoricalsAsync(Guid id)
        {
            return await _context.Components
                .Include(c => c.Historicals)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
       public async Task<PartComponent?> GetByIdWithAplsAsync(Guid id)
{
    return await _context.Components
        .Include(c => c.PartComponentAPLs)
            .ThenInclude(pca => pca.APL)
                .ThenInclude(apl => apl.Parts)
        .FirstOrDefaultAsync(c => c.Id == id);
}
    }
}
