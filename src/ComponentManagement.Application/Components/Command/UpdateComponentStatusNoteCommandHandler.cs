using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Components.Commands
{
    public class UpdateComponentStatusNoteCommandHandler
        : IRequestHandler<UpdateComponentStatusNoteCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IWhatsappNotificationService _whatsapp;

        public UpdateComponentStatusNoteCommandHandler(
            IAppDbContext context,
            IWhatsappNotificationService whatsapp)
        {
            _context = context;
            _whatsapp = whatsapp;
        }

        public async Task<bool> Handle(
            UpdateComponentStatusNoteCommand request,
            CancellationToken cancellationToken)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (component == null)
                return false;

            var oldStatus = component.Status;

            // Update Status
            if (request.Status.HasValue)
            {
                component.Status = request.Status.Value;
            }

            // Update Note
            if (!string.IsNullOrEmpty(request.Note))
            {
                component.Note = request.Note;
            }

            component.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            // ===============================
            // SEND WHATSAPP NOTIFICATION
            // ===============================
            if (request.Status.HasValue && oldStatus != component.Status)
            {
                await _whatsapp.SendUpdateStatusComponentAsync(component);
            }

            return true;
        }
    }
}
