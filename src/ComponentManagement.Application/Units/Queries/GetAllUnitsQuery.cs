using MediatR;
using ComponentManagement.Application.Units.Dtos;
namespace ComponentManagement.Application.Units.Queries
{
    public class GetAllUnitsQuery : IRequest<PagedResult<UnitDto>>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
    }
}
