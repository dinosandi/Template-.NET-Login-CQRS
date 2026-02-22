using ComponentManagement.Domain.Enums;
using System;

namespace ComponentManagement.Application.Units.Dtos
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<InstalledComponentDto> InstalledComponents { get; set; } = new();

    }
    //enpoint khusus
    public class UnitDashboardDto
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        public List<ComponentDashboardDto> Components { get; set; } = new();
    }

    public class ComponentDashboardDto
    {
        public Guid Id { get; set; }
        public string NamaKomponen { get; set; } = default!;
        public string Status { get; set; } = default!;
        public double RemainingHm { get; set; }
        public double UsedHm { get; set; }
        public double TotalLifetimeHm { get; set; }

    }
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public List<T> Items { get; set; } = new();
    }
    public class InstalledComponentDto
    {
        public Guid Id { get; set; }
        public string NamaKomponen { get; set; } = default!;
        public ComponentStatus Status { get; set; }
        public List<LifetimeDto> Lifetimes { get; set; } = new();
    }

    public class LifetimeDto
    {
        public double TotalLifetimeHm { get; set; }
        public double UsedLifetimeHm { get; set; }
        public double RemainingLifetimeHm { get; set; }
        public DateTimeOffset? InstalledAt { get; set; }
        public bool IsActive { get; set; }
    }
    public class UnitLifetimeDto
    {
        public Guid Id { get; set; }
        public string NamaKomponen { get; set; } = default!;
        public string Status { get; set; } = default!;


        public double TotalLifetimeHm { get; set; }
        public double UsedLifetimeHm { get; set; }
        public double RemainingLifetimeHm { get; set; }

        public DateTimeOffset? InstalledAt { get; set; }
    }
    


}
