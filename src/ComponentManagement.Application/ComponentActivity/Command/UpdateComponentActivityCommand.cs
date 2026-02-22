using MediatR;
using Microsoft.AspNetCore.Http;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.ComponentActivities.Commands;

public class UpdateComponentActivityCommand : IRequest<UpdateComponentActivityResponse>
{
    public Guid ComponentId { get; set; }
    public IFormFile? Documentation { get; set; } // PDF file
    public string? NeedSupport { get; set; }
    public ComponentStatus Status { get; set; }
}

public class UpdateComponentActivityResponse
{
    public Guid ActivityId { get; set; }
    public Guid ComponentId { get; set; }
    public string? DocumentationPath { get; set; }
    public ComponentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Tambahan data PartComponent
    public PartComponentDto Component { get; set; }
}

public class PartComponentDto
{
    public Guid Id { get; set; }
    public string NamaBrand { get; set; }
    public string NamaKomponen { get; set; }
    public string PartNumber { get; set; }
    public string NomerLaMbung { get; set; }
    public string? ImagePath { get; set; }
    public ComponentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Note { get; set; }
    
}

