using System;
using System.Collections.Generic;

namespace ComponentManagement.Domain.Entities
{
    public class APL
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relasi: 1 APL bisa punya banyak Part
        public ICollection<APLPart> Parts { get; set; } = new List<APLPart>();
        public ICollection<PartComponentAPL> PartComponentAPLs { get; set; } = new List<PartComponentAPL>();

        // 🔥 Relasi ke PartComponent
        public Guid? PartComponentId { get; set; }
        public PartComponent? PartComponent { get; set; }
    }
}
