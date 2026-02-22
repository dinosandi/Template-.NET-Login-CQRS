using System;
using MediatR;

namespace ComponentManagement.Application.Notifications.Commands
{
    public record CreateNotificationCommand(
    Guid ComponentId,
    string Title,
    string Message,
    string NamaKomponen,
    string? ImagePath,
    string? Note,
    Guid? UnitId,
    string? UnitName
) : IRequest;

}

