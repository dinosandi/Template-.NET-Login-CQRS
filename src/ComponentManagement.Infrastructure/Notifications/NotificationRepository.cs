using ComponentManagement.Application.Notifications.Interfaces;
using ComponentManagement.Domain.Notifications;
using ComponentManagement.Infrastructure.Persistence;

namespace ComponentManagement.Infrastructure.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Notification notification,
            CancellationToken cancellationToken)
        {
            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
