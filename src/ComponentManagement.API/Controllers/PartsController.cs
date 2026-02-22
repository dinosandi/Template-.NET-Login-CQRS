using MediatR;
using Microsoft.AspNetCore.Mvc;
using ComponentManagement.Application.Parts.Commands;
using ComponentManagement.Application.Parts.Queries;

namespace ComponentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PartsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePart([FromBody] CreatePartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllParts()
        {
            var result = await _mediator.Send(new GetAllPartQuery());
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePart(Guid id, [FromBody] UpdatePartCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(Guid id)
        {
            var result = await _mediator.Send(new DeletePartCommand { Id = id });
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
