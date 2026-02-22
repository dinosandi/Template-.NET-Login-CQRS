using ComponentManagement.Domain.Notifications;

namespace ComponentManagement.Application.Notifications.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
}
