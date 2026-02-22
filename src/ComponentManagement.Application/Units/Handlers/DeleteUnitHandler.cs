using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Units.Commands;
using ComponentManagement.Application.Exceptions;


namespace ComponentManagement.Application.Units.Handlers
{
    public class DeleteUnitHandler : IRequestHandler<DeleteUnitCommand, bool>
    {
        private readonly IAppDbContext _context;

        public DeleteUnitHandler(IAppDbContext context)
        {
            _context = context;
        }

     public async Task<bool> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
{
    var unit = await _context.Units
        .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

    if (unit == null)
        return false;

    var hasComponent = await _context.Components
        .AnyAsync(c => c.UnitId == unit.Id, cancellationToken);

    if (hasComponent)
        throw new BadRequestException(
            "Unit cannot be deleted because it still has components.");
    
    // jika berhasil dihapus, maka data unit akan hilang, tapi data component yang terkait dengan unit ini tetap aman karena sudah di-set null pada field UnitId
    


    _context.Units.Remove(unit);
    await _context.SaveChangesAsync(cancellationToken);

    return true;
}

    }
}

