using Microsoft.AspNetCore.Mvc;
using MediatR;
using ComponentManagement.Application.Components.Commands;
using ComponentManagement.Application.Components.Queries;
using ComponentManagement.API.Dtos;
using Microsoft.AspNetCore.Authorization;



namespace ComponentManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ComponentController(IMediator mediator) => _mediator = mediator;


        // CREATE COMPONENT
        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateComponentRequest request)
        {

            byte[]? fileData = null;
            string? fileName = null;

            if (request.Image != null)
            {
                using var ms = new MemoryStream();
                await request.Image.CopyToAsync(ms);
                fileData = ms.ToArray();
                fileName = request.Image.FileName;
            }

            var command = new CreateComponentCommand
            {
                PartId = request.PartId,
                NamaBrand = request.NamaBrand,
                NamaKomponen = request.NamaKomponen,
                PartNumber = request.PartNumber,
                NomerLaMbung = request.NomerLaMbung,
                Note = request.Note,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                FileData = fileData,
                FileName = fileName
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
       [HttpPost("{componentId}/install")]
public async Task<IActionResult> Install(
    Guid componentId,
    [FromBody] InstallComponentCommand command)
{
    command.PartComponentId = componentId;
    return Ok(await _mediator.Send(command));
}

        [Authorize]
        [HttpGet("{id:guid}/lifetime")]
        public async Task<IActionResult> GetLifetime(Guid id)
        {
            var result = await _mediator.Send(new GetComponentLifetimeQuery
            {
                ComponentId = id
            });

            return Ok(result);
        }


        //GetById
        [AllowAnonymous] // QR bisa dibuka tanpa login
        [HttpGet("by-token")]
        public async Task<IActionResult> GetByToken([FromQuery] string token)
        {
            var query = new GetComponentByTokenQuery(token);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { message = "Invalid or expired QR token" });

            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetComponentByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { message = "Component not found" });

            return Ok(result);
        }


        // GET ALL dengan pagination + filter
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllComponentsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        // UPDATE
        [Authorize]
        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid id, [FromForm] CreateComponentRequest request)
        {
            byte[]? fileData = null;
            string? fileName = null;

            if (request.Image != null)
            {
                using var ms = new MemoryStream();
                await request.Image.CopyToAsync(ms);
                fileData = ms.ToArray();
                fileName = request.Image.FileName;
            }

            var command = new UpdateComponentCommand
            {
                Id = id,
                NamaBrand = request.NamaBrand,
                NamaKomponen = request.NamaKomponen,
                PartNumber = request.PartNumber,
                NomerLaMbung = request.NomerLaMbung,
                Note = request.Note,
                Status = request.Status,
                FileData = fileData,
                FileName = fileName
            };

            var result = await _mediator.Send(command);
            if (!result) return NotFound(new { message = "Component not found" });

            return NoContent();
        }
        // PATCH
        [Authorize(Roles = "PE_Plant_Engineer,Planing_Install")]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatchComponent(Guid id, [FromBody] PatchComponentCommand command)
        {
            var request = new PatchComponentCommandWithId
            {
                Id = id,
                TanggalInstall = command.TanggalInstall,
                Status = command.Status,
                NomerLaMbung = command.NomerLaMbung
            };

            var result = await _mediator.Send(request);

            if (!result) return NotFound();
            return Ok();
        }
        [Authorize]
        [HttpPatch("{id:guid}/apl-data")]
        public async Task<IActionResult> PatchPartComponentAPL(Guid id, [FromBody] PatchPartComponentAPLCommand command)
        {
            if (id != command.PartComponentId)
                return BadRequest("PartComponentId in URL and body do not match");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Cek role manual
            if (!User.IsInRole("PE_Plant_Engineer"))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new { message = "Access denied. Your role does not have permission to delete this component." });
            }

            // Lanjut ke proses delete
            var command = new DeleteComponentCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { message = "Component not found" });

            return NoContent();
        }


        [Authorize]
        [HttpPatch("{componentId:guid}/unit")]
        public async Task<IActionResult> PatchComponentUnit(
     Guid componentId,
     [FromBody] PatchComponentUnitRequest request)
        {
            var command = new PatchComponentUnitCommandWithId
            {
                ComponentId = componentId,
                UnitId = request.UnitId,
                Note = request.Note
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }


        [Authorize]
        [HttpPatch("{id:guid}/status-note")]
        public async Task<IActionResult> UpdateStatusAndNote(Guid id, [FromBody] UpdateComponentStatusNoteCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID di URL dan body tidak sama.");

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { message = "Component tidak ditemukan." });

            return Ok(new { message = "Status dan Note berhasil diperbarui." });
        }
        [Authorize]
        [HttpPatch("{id:guid}/historical")]
        public async Task<IActionResult> UpdateHistorical(Guid id, [FromBody] PatchHistoricalCommand command)
        {
            if (id != command.PartComponentId)
                return BadRequest("PartComponentId in URL and body do not match");

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
