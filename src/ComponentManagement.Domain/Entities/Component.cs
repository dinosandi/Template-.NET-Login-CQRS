using System;
using ComponentManagement.Domain.Enums;
using ComponentManagement.Domain.Exceptions;


namespace ComponentManagement.Domain.Entities
{
    public class PartComponent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NamaBrand { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public string NomerLaMbung { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; } // nullable
        public string? ImagePath { get; set; } // path ke file image

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }

        public Guid? PartId { get; set; }
        public Part? Part { get; set; }
        public string? QrBase64 { get; set; }
        public string? QrImageUrl { get; set; }
        public string? QrToken { get; set; }


        public ComponentStatus Status { get; set; } = ComponentStatus.WIP; // default WIPpublic ICollection<ComponentActivity> ComponentActivities { get; set; } = new List<ComponentActivity>();
        public ICollection<ComponentActivity> ComponentActivities { get; set; } = new List<ComponentActivity>();
        public ICollection<ComponentCustomPart> CustomParts { get; set; } = new List<ComponentCustomPart>();
        public ICollection<Historical> Historicals { get; set; } = new List<Historical>();

        public ICollection<APL> APLs { get; set; } = new List<APL>();
        public ICollection<PartComponentAPL> PartComponentAPLs { get; set; }
        public ICollection<ComponentLifetime> ComponentLifetimes { get; set; }
        public Guid? UnitId { get; private set; }
        public Unit? Unit { get; private set; }
        // =========================
        // DOMAIN METHOD
        // =========================
        public void RequestInstallation(Guid unitId, string? note)
        {
            UnitId = unitId;
            Status = ComponentStatus.Requested;

            if (!string.IsNullOrWhiteSpace(note))
                Note = note;

            UpdatedAt = DateTime.UtcNow;
        }
        public void Install(DateTimeOffset installedAt)
        {

            Status = ComponentStatus.INSTALLED;
            TanggalInstall = installedAt;
            UpdatedAt = DateTime.UtcNow; // Log audit internal tetap UTC
        }

    }
}
