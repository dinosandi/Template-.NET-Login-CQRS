using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Notifications.Commands;
using ComponentManagement.Domain.Exceptions;

namespace ComponentManagement.Application.Notifications.Handlers
{
  public class MarkNotificationAsReadHandler
    : IRequestHandler<MarkNotificationAsReadCommand>
{
    private readonly IAppDbContext _context;

    public MarkNotificationAsReadHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken ct)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == request.Id, ct);

        if (notification == null)
            throw new KeyNotFoundException("Notification not found");

        notification.IsRead = true;

        await _context.SaveChangesAsync(ct);
    }
}

}
