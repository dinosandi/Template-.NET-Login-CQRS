using ComponentManagement.Application.Components.Dtos;
using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Queries
{
    public class GetAllComponentsQuery : IRequest<PaginatedResult<ComponentDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // filter opsional
        public string? NamaBrand { get; set; }
        public string? NamaKomponen { get; set; }
        public string? PartNumber { get; set; }
        public DateTimeOffset? TanggalInstall { get; set; }
        public ComponentStatus? Status { get; set; }
    }

    public class PaginatedResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; } = new();
        public ComponentStatusSummaryDto StatusSummary { get; set; } = new ComponentStatusSummaryDto();
    }
}
