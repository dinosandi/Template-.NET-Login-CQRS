using Microsoft.AspNetCore.Http;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.API.Dtos
{
    public class CreateComponentRequest
    {
        public Guid? PartId { get; set; }
        public IFormFile? Image { get; set; }
        public string NamaBrand { get; set; }
        public string NamaKomponen { get; set; }
        public string PartNumber { get; set; }
        public string NomerLaMbung { get; set; }
        public DateTime? TanggalInstall { get; set; }
        public string Note { get; set; }
        public ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
