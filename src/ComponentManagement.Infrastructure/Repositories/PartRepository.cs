using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ComponentManagement.Infrastructure.Repositories;

public class PartRepository : IPartRepository
{
    private readonly AppDbContext _context;

    public PartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Part part)
    {
        await _context.Parts.AddAsync(part);
    }
    public async Task<IEnumerable<Part>> GetAllAsync()
    {
        return await _context.Parts.ToListAsync();
    }
    public async Task<Part?> GetByIdAsync(Guid id)
    {
        return await _context.Parts.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Delete(Part part)
    {
        _context.Parts.Remove(part);
    }
}


