using System;
using System.Collections.Generic;

namespace ComponentManagement.Application.APLs.Queries
{
    public class APLDto
    {
        public Guid Id { get; set; }
        public string NameBrand { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // List Part di dalam APL
        public List<APLPartDto> Parts { get; set; } = new();
    }

    public class APLPartDto
    {
        public Guid Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string? Priority { get; set; }
        public int Quantity { get; set; }
    }
}
