using System;

namespace ComponentManagement.Domain.Entities
{
    public class Historical
    {
        public Guid Id { get; set; }

         // Relasi ke PartComponent
        public Guid PartComponentId { get; set; }
        public PartComponent PartComponent { get; set; }
        public DateTimeOffset TanggalRFU { get; set; }
        public string OldCodeNumber { get; set; }
        public DateTimeOffset TanggalInstall { get; set; }
        public string NewCodeNumber { get; set; }
        public string Hm { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
