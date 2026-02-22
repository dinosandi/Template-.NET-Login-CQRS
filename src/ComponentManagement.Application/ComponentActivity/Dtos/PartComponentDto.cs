using System;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Common.Dtos
{
    public class PartComponentDto
    {
        public Guid Id { get; set; }
        public string NamaBrand { get; set; } = string.Empty;
        public string NamaKomponen { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string NomerLaMbung { get; set; } = string.Empty;
        public string QrToken { get; set; } 
        public string? ImagePath { get; set; }
        public ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Note { get; set; }
    }
}
