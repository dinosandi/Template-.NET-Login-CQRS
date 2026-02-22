using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.APLs.Commands
{
    public class DeleteAPLCommandHandler : IRequestHandler<DeleteAPLCommand, bool>
    {
        private readonly IAppDbContext _context;

        public DeleteAPLCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAPLCommand request, CancellationToken cancellationToken)
        {
            var apl = await _context.APLs
                .Include(a => a.PartComponentAPLs) // include relasi
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (apl == null) return false;

            // 🔹 hapus anaknya dulu (kalau pakai Restrict)

            // 🔹 hapus parent
            _context.APLs.Remove(apl);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
