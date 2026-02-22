using System;

namespace ComponentManagement.Domain.Entities
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public ICollection<PartComponent> PartComponents { get; set; }
        public ICollection<ComponentLifetime> ComponentLifetimes { get; set; }
    }
}
