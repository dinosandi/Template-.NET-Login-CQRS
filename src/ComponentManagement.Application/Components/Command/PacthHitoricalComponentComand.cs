using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchHistoricalCommand : IRequest<PatchHistoricalResponse>
    {
        public Guid PartComponentId { get; set; }
        public string NewCodeNumber { get; set; }
        public string Action { get; set; }
        public DateTime? TanggalRFU { get; set; }
    }

    public class PatchHistoricalResponse
    {
        public Guid PartComponentId { get; set; }
        public string Message { get; set; }
        public int TotalHistorical { get; set; }
        public HistoricalDto LatestHistorical { get; set; }
    }

    public class HistoricalDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset TanggalRFU { get; set; }
        public string OldCodeNumber { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; }
        public string NewCodeNumber { get; set; }
        public string Hm { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class UpdateComponentStatusWithHmCommand : IRequest<bool>
    {
        public Guid ComponentId { get; set; }
        public ComponentStatus NewStatus { get; set; }
        public DateTime StatusChangedAt { get; set; } // biasanya DateTime.UtcNow
        public string? NewCodeNumber { get; set; } // CN baru (opsional)
        public string Action { get; set; }
    }

}
