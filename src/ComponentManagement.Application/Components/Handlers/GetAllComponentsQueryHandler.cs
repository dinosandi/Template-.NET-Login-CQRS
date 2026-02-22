using ComponentManagement.Application.Components.Dtos;
using ComponentManagement.Application.Components.Queries;
using ComponentManagement.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Domain.Enums;
using ComponentManagement.Application.Common;

namespace ComponentManagement.Application.Components.Handlers
{
    public class GetAllComponentsQueryHandler : IRequestHandler<GetAllComponentsQuery, PaginatedResult<ComponentDto>>
    {
        private readonly IAppDbContext _context;

        public GetAllComponentsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<ComponentDto>> Handle(GetAllComponentsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Components
                .Include(c => c.ComponentActivities)
                .Include(c => c.Unit)
                .Include(c => c.ComponentLifetimes)
                .Include(c => c.PartComponentAPLs)
                    .ThenInclude(pca => pca.APL)
                        .ThenInclude(apl => apl.Parts)
                .Include(c => c.CustomParts)
                .Include(c => c.Historicals)
                .AsNoTracking()
                .AsQueryable();

            // 🔹 Filtering
            if (!string.IsNullOrEmpty(request.NamaBrand))
                query = query.Where(c => c.NamaBrand.Contains(request.NamaBrand));

            if (!string.IsNullOrEmpty(request.NamaKomponen))
                query = query.Where(c => c.NamaKomponen.Contains(request.NamaKomponen));

            if (!string.IsNullOrEmpty(request.PartNumber))
                query = query.Where(c => c.PartNumber.Contains(request.PartNumber));

            if (request.Status.HasValue)
                query = query.Where(c => c.Status == request.Status.Value);

            if (request.TanggalInstall.HasValue)
                query = query.Where(c => c.TanggalInstall == request.TanggalInstall.Value);

            // 🔹 Total data
            var totalCount = await query.CountAsync(cancellationToken);

            // 🔹 Summary status
            var rfuCount = await _context.Components.CountAsync(c => c.Status == ComponentStatus.RFU, cancellationToken);
            var wipCount = await _context.Components.CountAsync(c => c.Status == ComponentStatus.WIP, cancellationToken);

            // 🔹 Paging
            var components = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);



            // 🔹 Mapping ke DTO
            var items = components.Select(component => new ComponentDto
            {
                Id = component.Id,
                NamaBrand = component.NamaBrand,
                NamaKomponen = component.NamaKomponen,
                PartNumber = component.PartNumber,
                TanggalInstall = component.TanggalInstall,
                ImagePath = component.ImagePath,
                NomerLaMbung = component.NomerLaMbung,
                Note = component.Note,
                Status = component.Status,
                QrToken = component.QrToken,
                CreatedAt = component.CreatedAt,
                UpdatedAt = component.UpdatedAt,

                // 🔸 Unit
                Unit = component.Unit == null ? null : new List<UnitDto>
                {
                    new UnitDto
                    {
                        Id = component.Unit.Id,
                        NameUnit = component.Unit.NameUnit,
                    }
                },

                // Lifetime
                 ComponentLifetimes = component.ComponentLifetimes
                    .Where(cl => cl.IsActive)
                    .Select(cl =>
                    {
                        var used = LifetimeCalculator.CalculateUsedHm(cl.InstalledAt.Value);
                        var remaining = LifetimeCalculator.CalculateRemainingHm(
                            cl.TotalLifetimeHm,
                            cl.InstalledAt.Value);

                        return new ComponentLifetimeDto
                        {
                            Id = cl.Id,
                            TotalLifetimeHm = cl.TotalLifetimeHm,
                            UsedLifetimeHm = used,
                            RemainingLifetimeHm = remaining,
                            InstalledAt = cl.InstalledAt,
                            CreatedAt = cl.CreatedAt,
                            UpdatedAt = cl.UpdatedAt
                        };
                    }).ToList(),

                // 🔸 APL dan APLPart
                APLs = component.PartComponentAPLs
                    .Select(pca => new APLDto
                    {
                        Id = pca.APL.Id,
                        NameBrand = pca.APL.NameBrand,
                        Parts = pca.APL.Parts.Select(p => new APLPartDto
                        {
                            PartNumber = p.PartNumber,
                            Description = p.Description,
                            Priority = p.Priority,
                            Quantity = p.Quantity
                        }).ToList()
                    }).ToList(),

                // 🔸 Historical
                Historicals = component.Historicals
                    .Select(h => new HistoricalDto
                    {
                        Id = h.Id,
                        OldCodeNumber = h.OldCodeNumber,
                        NewCodeNumber = h.NewCodeNumber,
                        Hm = h.Hm,
                        TanggalRFU = h.TanggalRFU,
                        TanggalInstall = h.TanggalInstall,
                        Action = h.Action,
                        CreatedAt = h.CreatedAt,
                        UpdatedAt = h.UpdatedAt

                    }).ToList(),

                // 🔸 ComponentCustomParts
                CustomParts = component.CustomParts
                    .Select(cp => new ComponentCustomPartDto
                    {
                        Id = cp.Id,
                        NameBrand = cp.NameBrand,
                        PartNumber = cp.PartNumber,
                        Description = cp.Description,
                        Priority = cp.Priority,
                        Quantity = cp.Quantity
                    }).ToList(),


                // 🔸 ComponentActivities
                ComponentActivities = component.ComponentActivities
                    .Select(act => new ComponentActivityDto
                    {
                        Id = act.Id,
                        Documentation = act.Documentation,
                        NeedSupport = act.NeedSupport,
                        Status = act.Status,
                        CreatedAt = act.CreatedAt
                    }).ToList()
            }).ToList();

            // 🔹 Hasil akhir
            return new PaginatedResult<ComponentDto>
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = items,
                StatusSummary = new ComponentStatusSummaryDto
                {
                    RFUCount = rfuCount,
                    WIPCount = wipCount,
                    INSTALLEDCount = totalCount - rfuCount - wipCount
                }
            };
        }
    }
}
