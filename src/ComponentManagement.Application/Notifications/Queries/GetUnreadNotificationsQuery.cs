using MediatR;
using ComponentManagement.Application.Notifications.Dtos;

namespace ComponentManagement.Application.Notifications.Queries
{
    public class GetUnreadNotificationsQuery 
        : IRequest<List<NotificationDto>>
    {
        // optional: nanti bisa ditambah UserId / Role
    }
}
