using System;

namespace ComponentManagement.Domain.Entities
{
    public class Part
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }

        public ICollection<PartComponent> Components { get; set; } = new List<PartComponent>();
    }
}
