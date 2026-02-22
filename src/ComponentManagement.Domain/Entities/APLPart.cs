using System;

namespace ComponentManagement.Domain.Entities
{
    public class APLPart
    {
        public Guid Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string? Priority { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public Guid APLId { get; set; }
        public APL APL { get; set; }
    }
}
