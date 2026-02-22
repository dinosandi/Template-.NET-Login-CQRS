using MediatR;
using ComponentManagement.Application.Components.Dtos;
using ComponentManagement.Application.Components.Queries;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Components.Handlers
{
    public class GetComponentByIdQueryHandler
        : IRequestHandler<GetComponentByIdQuery, ComponentDto?>
    {
        private readonly IAppDbContext _context;

        public GetComponentByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<ComponentDto?> Handle(GetComponentByIdQuery request, CancellationToken cancellationToken)
        {
            var component = await _context.Components
                .Include(c => c.Unit)
                .Include(c => c.ComponentActivities)
                .Include(c => c.PartComponentAPLs)
                    .ThenInclude(pca => pca.APL)
                        .ThenInclude(apl => apl.Parts)
                .Include(c => c.CustomParts)
                .Include(c => c.Historicals)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (component == null)
                return null;

            return new ComponentDto
            {
                Id = component.Id,
                NamaBrand = component.NamaBrand,
                NamaKomponen = component.NamaKomponen,
                PartNumber = component.PartNumber,
                TanggalInstall = component.TanggalInstall,
                ImagePath = component.ImagePath,
                NomerLaMbung = component.NomerLaMbung,
                Note = component.Note,
                QrToken = component.QrToken,
                Status = component.Status,
                CreatedAt = component.CreatedAt,
                UpdatedAt = component.UpdatedAt,

                Unit = component.Unit == null ? null : new List<UnitDto>
                {
                    new UnitDto
                    {
                        Id = component.Unit.Id,
                        NameUnit = component.Unit.NameUnit,
                    }
                },

                APLs = component.PartComponentAPLs.Select(pca => new APLDto
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

                CustomParts = component.CustomParts.Select(cp => new ComponentCustomPartDto
                {
                    Id = cp.Id,
                    NameBrand = cp.NameBrand,
                    PartNumber = cp.PartNumber,
                    Description = cp.Description,
                    Priority = cp.Priority,
                    Quantity = cp.Quantity
                }).ToList(),

                Historicals = component.Historicals.Select(h => new HistoricalDto
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

                ComponentActivities = component.ComponentActivities.Select(act => new ComponentActivityDto
                {
                    Id = act.Id,
                    Documentation = act.Documentation,
                    NeedSupport = act.NeedSupport,
                    Status = act.Status,
                    CreatedAt = act.CreatedAt
                }).ToList()
            };
        }
    }
}
