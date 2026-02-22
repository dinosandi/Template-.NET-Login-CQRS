using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Units.Dtos;
using ComponentManagement.Domain.Enums;
using ComponentManagement.Application.Common;

namespace ComponentManagement.Application.Units.Queries
{
    public record GetUnitByIdQuery(Guid Id) : IRequest<UnitDashboardDto?>;

    public class GetUnitByIdQueryHandler
        : IRequestHandler<GetUnitByIdQuery, UnitDashboardDto?>
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitByIdQueryHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDashboardDto?> Handle(
      GetUnitByIdQuery request,
      CancellationToken cancellationToken)
        {
            var rawData = await _unitRepository.GetQueryable()
                .AsNoTracking()
                .Where(u => u.Id == request.Id)
                .Select(u => new
                {
                    u.Id,
                    u.NameUnit,
                    Components = u.PartComponents
                        .Where(pc => pc.Status == ComponentStatus.INSTALLED)
                        .Select(pc => new
                        {
                            pc.Id,
                            pc.NamaKomponen,
                            Status = pc.Status.ToString(),
                            Lifetime = pc.ComponentLifetimes
                                .Where(l => l.IsActive && l.InstalledAt != null)
                                .OrderByDescending(l => l.InstalledAt)
                                .Select(l => new
                                {
                                    l.TotalLifetimeHm,
                                    l.InstalledAt
                                })
                                .FirstOrDefault()
                        })
                        .Where(c => c.Lifetime != null)
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (rawData == null)
                return null;

            // ⬇️ IN-MEMORY CALCULATION (AMAN)
            return new UnitDashboardDto
            {
                Id = rawData.Id,
                NameUnit = rawData.NameUnit,
                Components = rawData.Components
                    .Select(c => new ComponentDashboardDto
                    {
                        Id = c.Id,
                        NamaKomponen = c.NamaKomponen,
                        Status = c.Status,

                        TotalLifetimeHm = c.Lifetime!.TotalLifetimeHm,
                        UsedHm = LifetimeCalculator.CalculateUsedHm(
                            c.Lifetime.InstalledAt!.Value
                        ),
                        RemainingHm = LifetimeCalculator.CalculateRemainingHm(
                            c.Lifetime.TotalLifetimeHm,
                            c.Lifetime.InstalledAt!.Value
                        )
                    })
                    .ToList()
            };
        }
    }
}
