using Microsoft.AspNetCore.Mvc;
using MediatR;
using ComponentManagement.Application.Units.Commands;
using ComponentManagement.Application.Units.Queries;
using Microsoft.AspNetCore.Authorization;
using ComponentManagement.Application.Units.Dtos;

namespace ComponentManagement.API.Controllers
{
    [ApiController]
    [Route("api/unit")]
    public class UnitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UnitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUnitCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("lifetime")]
        public async Task<ActionResult<PagedResult<UnitDashboardDto>>> GetDashboard([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var query = new GetUnitDashboardQuery(page, pageSize);
            return Ok(await _mediator.Send(query));
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UnitDashboardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Mengirim query ke MediatR Handler
            var query = new GetUnitByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new { Message = $"Unit dengan ID {id} tidak ditemukan." });
            }

            return Ok(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUnitsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUnitCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteUnitCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
