using MediatR;
using Microsoft.AspNetCore.Mvc;
using ComponentManagement.Application.Notifications.Queries;
using ComponentManagement.Application.Notifications.Commands;

namespace ComponentManagement.API.Controllers
{
    [ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUnread()
    {
        var result = await _mediator.Send(new GetUnreadNotificationsQuery());
        return Ok(result);
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _mediator.Send(new MarkNotificationAsReadCommand(id));
        return NoContent();
    }
}

}
