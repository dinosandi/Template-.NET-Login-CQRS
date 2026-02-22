using ComponentManagement.Domain.Entities; // Ensure this is the correct namespace for ComponentLifetime

namespace ComponentManagement.Application.Interfaces
{
    
public interface ILifetimeNotificationService
{
    Task SendLifetimeWarningAsync(
        ComponentLifetime lifetime,
        int threshold,
        CancellationToken cancellationToken);
}
}


