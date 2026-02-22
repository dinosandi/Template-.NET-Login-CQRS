using MediatR;
using System;

namespace ComponentManagement.Application.ComponentActivities.Queries
{
    public class GetComponentActivityByIdQuery : IRequest<GetComponentActivityByIdResponse>
    {
        public Guid ActivityId { get; set; }
    }

    public class GetComponentActivityByIdResponse
    {
        public Guid ActivityId { get; set; }
        public Guid ComponentId { get; set; }
        public string? DocumentationPath { get; set; } // output name
        public string? NeedSupport { get; set; } // output name
        public Domain.Enums.ComponentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Common.Dtos.PartComponentDto Component { get; set; } = null!;
    }
}
