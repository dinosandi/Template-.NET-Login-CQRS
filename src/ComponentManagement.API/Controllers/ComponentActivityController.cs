using Microsoft.AspNetCore.Mvc;
using MediatR;
using ComponentManagement.Application.ComponentActivities.Commands;
using ComponentManagement.API.Dtos;
using ComponentManagement.Application.ComponentActivities.Queries;
using Microsoft.AspNetCore.Authorization;


namespace ComponentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentActivityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ComponentActivityController(IMediator mediator) => _mediator = mediator;


        // UPDATE / CREATE Activity (karena setiap update bikin record baru)
        [Authorize]
        [HttpPost("{componentId:guid}/activities")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddActivity(Guid componentId, [FromForm] UpdateComponentActivityRequest request)
        {
            var command = new UpdateComponentActivityCommand
            {
                ComponentId = componentId,
                Documentation = request.Documentation,
                NeedSupport = request.NeedSupport,
                Status = request.Status
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    // GET BY ID
    [Authorize]
        [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetComponentActivityByIdQuery { ActivityId = id });
        if (result == null) return NotFound();
        return Ok(result);
    }
    // GET ALL with pagination and filtering
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllComponentActivitiesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
        
    }
}
