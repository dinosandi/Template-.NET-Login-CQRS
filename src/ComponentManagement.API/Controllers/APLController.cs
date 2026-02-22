using Microsoft.AspNetCore.Mvc;
using MediatR;
using ComponentManagement.Application.APLs.Commands;
using ComponentManagement.Application.APLs.Queries;
using Microsoft.AspNetCore.Authorization;


namespace ComponentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APLController : ControllerBase
    {
        private readonly IMediator _mediator;
        public APLController(IMediator mediator) => _mediator = mediator;
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAPLQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAPL([FromBody] CreateAPLCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<APLDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAPLWithPartsQuery(id));
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAPLCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAPLCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
