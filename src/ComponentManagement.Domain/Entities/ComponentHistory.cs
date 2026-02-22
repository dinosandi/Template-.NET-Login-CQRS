using System;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Domain.Entities
{
    public class ComponentHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ComponentId { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public DateTimeOffset? OldTanggalInstall { get; set; }
        public DateTimeOffset? NewTanggalInstall { get; set; }

        public ComponentStatus? OldStatus { get; set; }
        public ComponentStatus? NewStatus { get; set; }

        public string? OldNomerLaMbung { get; set; }
        public string? NewNomerLaMbung { get; set; }

        // navigasi opsional
        public PartComponent Component { get; set; }
    }
}
