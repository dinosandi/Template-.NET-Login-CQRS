using ComponentManagement.Application.Notifications.Interfaces;
using Microsoft.AspNetCore.SignalR;
namespace ComponentManagement.Infrastructure.Realtime;

public class SignalRNotificationBroadcaster 
    : INotificationBroadcaster
{
    private readonly IHubContext<NotificationHub> _hub;

    public SignalRNotificationBroadcaster(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public async Task BroadcastAsync(object payload, CancellationToken ct)
    {
        await _hub.Clients.All.SendAsync("notification:new", payload, ct);
    }
}
