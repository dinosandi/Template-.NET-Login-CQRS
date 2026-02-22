using ComponentManagement.Application.Notifications.Commands;
using ComponentManagement.Application.Notifications.Interfaces;
using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Notifications;

namespace ComponentManagement.Application.Notifications.Handlers
{
   public class CreateNotificationHandler 
    : IRequestHandler<CreateNotificationCommand>
{
    private readonly IAppDbContext _context;
    private readonly INotificationBroadcaster _broadcaster;

    public CreateNotificationHandler(
        IAppDbContext context,
        INotificationBroadcaster broadcaster)
    {
        _context = context;
        _broadcaster = broadcaster;
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken ct)
    {
        var notif = new Notification
        {
            Id = Guid.NewGuid(),
            ComponentId = request.ComponentId,
            Title = request.Title,
            Message = request.Message,
            NamaKomponen = request.NamaKomponen,
            ImagePath = request.ImagePath,
            Note = request.Note,
            UnitId = request.UnitId,    
            UnitName = request.UnitName,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notif);
        await _context.SaveChangesAsync(ct);

        // ✅ Application hanya memanggil interface
        await _broadcaster.BroadcastAsync(notif, ct);
    }
}

}

