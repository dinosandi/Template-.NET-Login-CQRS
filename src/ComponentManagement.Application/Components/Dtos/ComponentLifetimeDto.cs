using System;

namespace ComponentManagement.Application.Components.Dtos
{
    public class ComponentLifetimeDto
    {
        public Guid Id { get; set; }
        public double TotalLifetimeHm { get; set; }
        public double UsedLifetimeHm { get; set; }
        public double RemainingLifetimeHm { get; set; }
        public DateTimeOffset? InstalledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
