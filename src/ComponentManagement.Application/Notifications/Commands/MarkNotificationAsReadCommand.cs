using MediatR;

namespace ComponentManagement.Application.Notifications.Commands

{
       public record MarkNotificationAsReadCommand(Guid Id) : IRequest;
}
