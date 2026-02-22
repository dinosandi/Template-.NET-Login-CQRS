using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Dtos
{
    public class ComponentDto
    {
        public Guid Id { get; set; }
        public string NamaBrand { get; set; }
        public string NamaPart { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; }
        public string? ImagePath { get; set; }
        public string NomerLaMbung { get; set; }
        public string Note { get; set; }
        public string QrToken { get; set; }
        public ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 🔹 Relasi ke APL
        public List<APLDto> APLs { get; set; } = new();

        // 🔹 Relasi ke ComponentActivity
        public List<ComponentActivityDto> ComponentActivities { get; set; } = new();
        // 🔹 Relasi ke ComponentCustomPart
        public List<ComponentCustomPartDto> CustomParts { get; set; } = new();
        // 🔹 Relasi ke Historical
        public List<HistoricalDto> Historicals { get; set; } = new();
        public List<UnitDto> Unit { get; set; }
        public List<ComponentLifetimeDto> ComponentLifetimes { get; set; }

    }

    public class ComponentStatusSummaryDto
    {
        public int RFUCount { get; set; }
        public int WIPCount { get; set; }
        public int INSTALLEDCount { get; set; }
    }

    // DTO untuk APL
    public class APLDto
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public List<APLPartDto> Parts { get; set; } = new();
    }

    public class APLPartDto
    {
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string? Priority { get; set; }
        public int Quantity { get; set; }
    }

    // DTO untuk ComponentActivity
    public class ComponentActivityDto
    {
        public Guid Id { get; set; }
        public string? Documentation { get; set; }
        public string? NeedSupport { get; set; }
        public ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ComponentCustomPartDto
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string? Priority { get; set; }
        public int Quantity { get; set; }
    }
    public class HistoricalDto
    {
        public Guid Id { get; set; }
        public string OldCodeNumber { get; set; }
        public string NewCodeNumber { get; set; }
        public string Hm { get; set; }
        public string Action { get; set; }
        public DateTimeOffset TanggalRFU { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UnitDto
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; }
    }
    public class lifetimeDto
    {
        public Guid Id { get; set; }
        public double TotalLifetimeHm { get; set; }   // contoh: 4000

        // 🔹 DISET SAAT STATUS = INSTALLED
        public DateTime InstalledAt { get; set; }

        // 🔹 CACHE (opsional, tapi recommended)
        public double UsedLifetimeHm { get; set; }
        public double RemainingLifetimeHm { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
