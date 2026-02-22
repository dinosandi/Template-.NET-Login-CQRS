using System;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Domain.Entities;

public class ComponentActivity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ComponentId { get; set; } // relasi ke PartComponent
    public PartComponent Component { get; set; }
    public string? Documentation { get; set; } // bisa path dokumen/file
    public string? NeedSupport { get; set; } // tambahan part/ support
    public ComponentStatus Status { get; set; } = ComponentStatus.WIP; // default WIP

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

