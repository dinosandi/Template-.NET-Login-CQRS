using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class CreateComponentCommand : IRequest<CreateComponentResponse>
    {
        public Guid? PartId { get; set; }
        public string NamaBrand { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public string NomerLaMbung { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; } // nullable
        public string? Note { get; set; }
        public ComponentStatus Status { get; set; } = ComponentStatus.WIP; // default WIP
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // DTO untuk file
        public byte[]? FileData { get; set; }
        public string? FileName { get; set; }
    }

    public class CreateComponentResponse
    {
        public Guid Id { get; set; }
        public Guid? PartId { get; set; }
        public string NamaBrand { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public string NomerLaMbung { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; }
        public string? ImagePath { get; set; }
        public string? Note { get; set; }
        public string? QrBase64 { get; set; }
        public string? QrImageUrl { get; set; }
        public string? QrToken { get; set; }

        public ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
