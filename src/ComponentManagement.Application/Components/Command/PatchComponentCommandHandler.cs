using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchComponentCommandHandler : IRequestHandler<PatchComponentCommandWithId, bool>
    {
        private readonly IAppDbContext _context;

        public PatchComponentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(PatchComponentCommandWithId request, CancellationToken cancellationToken)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (component == null) return false;

            var history = new ComponentHistory
            {
                ComponentId = component.Id,
                OldTanggalInstall = component.TanggalInstall,
                NewTanggalInstall = request.TanggalInstall ?? component.TanggalInstall,
                OldStatus = component.Status,
                NewStatus = request.Status ?? component.Status,
                OldNomerLaMbung = component.NomerLaMbung,
                NewNomerLaMbung = request.NomerLaMbung ?? component.NomerLaMbung
            };

            if (request.TanggalInstall.HasValue)
                component.TanggalInstall = request.TanggalInstall.Value;

            if (request.Status.HasValue)
                component.Status = request.Status.Value;

            if (!string.IsNullOrWhiteSpace(request.NomerLaMbung))
                component.NomerLaMbung = request.NomerLaMbung;

            component.UpdatedAt = DateTime.UtcNow;

            _context.ComponentHistories.Add(history);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
