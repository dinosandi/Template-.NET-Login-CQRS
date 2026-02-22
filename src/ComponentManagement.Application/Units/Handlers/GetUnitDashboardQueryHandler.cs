using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Units.Dtos;
using ComponentManagement.Domain.Enums;
using ComponentManagement.Application.Common;

namespace ComponentManagement.Application.Units.Queries
{
    public record GetUnitDashboardQuery(int? Page, int? PageSize)
        : IRequest<PagedResult<UnitDashboardDto>>;

    public class GetUnitDashboardQueryHandler
        : IRequestHandler<GetUnitDashboardQuery, PagedResult<UnitDashboardDto>>
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitDashboardQueryHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<PagedResult<UnitDashboardDto>> Handle(
            GetUnitDashboardQuery request,
            CancellationToken cancellationToken)
        {
            var baseQuery = _unitRepository.GetQueryable()
                .AsNoTracking();

            var dashboardQuery = baseQuery
                .Select(u => new UnitDashboardDto
                {
                    Id = u.Id,
                    NameUnit = u.NameUnit,
                    Components = u.PartComponents
                        .Where(pc => pc.Status == ComponentStatus.INSTALLED)
                        .Select(pc => pc.ComponentLifetimes
                            .Where(l => l.IsActive && l.InstalledAt != null)
                            .OrderByDescending(l => l.InstalledAt)
                            .Select(l => new ComponentDashboardDto
                            {
                                Id = pc.Id,
                                NamaKomponen = pc.NamaKomponen,
                                Status = pc.Status.ToString(),

                                TotalLifetimeHm = l.TotalLifetimeHm,
                                UsedHm = LifetimeCalculator.CalculateUsedHm(l.InstalledAt!.Value),
                                RemainingHm = LifetimeCalculator.CalculateRemainingHm(
                                    l.TotalLifetimeHm,
                                    l.InstalledAt!.Value
                                )
                            })
                            .FirstOrDefault()
                        )
                        .Where(c => c != null)
                        .ToList()!
                })
                .Where(u => u.Components.Any());

            var totalCount = await dashboardQuery.CountAsync(cancellationToken);

            if (request.Page.HasValue && request.PageSize.HasValue)
            {
                dashboardQuery = dashboardQuery
                    .Skip((request.Page.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value);
            }

            return new PagedResult<UnitDashboardDto>
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Items = await dashboardQuery.ToListAsync(cancellationToken)
            };
        }
    }
}
