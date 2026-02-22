using ComponentManagement.Domain.Notifications;

namespace ComponentManagement.Application.Notifications.Interfaces;


public interface INotificationBroadcaster
{
    Task BroadcastAsync(object payload, CancellationToken cancellationToken);
    
}
