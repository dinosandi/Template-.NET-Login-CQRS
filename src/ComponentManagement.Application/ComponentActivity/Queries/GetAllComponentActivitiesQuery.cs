using MediatR;
using System.Collections.Generic;

namespace ComponentManagement.Application.ComponentActivities.Queries
{
    public class GetAllComponentActivitiesQuery : IRequest<GetAllComponentActivitiesResponse>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // optional filters
        public Domain.Enums.ComponentStatus? Status { get; set; }
        public string? Search { get; set; }
    }

    public class GetAllComponentActivitiesResponse
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ComponentActivityItemDto> Items { get; set; } = new List<ComponentActivityItemDto>();
    }

    public class ComponentActivityItemDto
    {
        public Guid ActivityId { get; set; }
        public Guid ComponentId { get; set; }
        public string? DocumentationPath { get; set; }
        public string? NeedSupport { get; set; } // output name
        public Domain.Enums.ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Common.Dtos.PartComponentDto Component { get; set; } = null!;
    }
}
