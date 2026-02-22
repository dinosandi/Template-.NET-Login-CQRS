using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Units.Dtos;

namespace ComponentManagement.Application.Units.Queries
{
    public class GetAllUnitsQueryHandler
        : IRequestHandler<GetAllUnitsQuery, PagedResult<UnitDto>>
    {
        private readonly IUnitRepository _unitRepository;

        public GetAllUnitsQueryHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<PagedResult<UnitDto>> Handle(
            GetAllUnitsQuery request,
            CancellationToken cancellationToken)
        {
            var units = await _unitRepository.GetAllAsync();
            var query = units.AsQueryable();

            // =========================
            // 🔍 SEARCH BY NAME UNIT
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var keyword = request.Search.Trim().ToLower();

                query = query.Where(u =>
                    u.NameUnit.ToLower().Contains(keyword)
                );
            }

            var totalCount = query.Count();

            // =========================
            // 📄 PAGINATION (OPTIONAL)
            // =========================
            if (request.Page.HasValue && request.PageSize.HasValue)
            {
                var skip = (request.Page.Value - 1) * request.PageSize.Value;

                query = query
                    .Skip(skip)
                    .Take(request.PageSize.Value);
            }

            var items = query.Select(u => new UnitDto
            {
                Id = u.Id,
                NameUnit = u.NameUnit,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            return new PagedResult<UnitDto>
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Items = items
            };
        }
    }
}
