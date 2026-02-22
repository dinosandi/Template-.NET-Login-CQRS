using MediatR;
using ComponentManagement.Application.Components.Dtos;
using ComponentManagement.Application.Components.Queries;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Components.Handlers
{
    public class GetComponentByTokenQueryHandler
        : IRequestHandler<GetComponentByTokenQuery, ComponentDto?>
    {
        private readonly IAppDbContext _context;
        private readonly ITokenService _tokenService;

        public GetComponentByTokenQueryHandler(
            IAppDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ComponentDto?> Handle(GetComponentByTokenQuery request, CancellationToken cancellationToken)
        {
            // ============================
            // 1️⃣ Cari token di database
            // ============================
            var component = await _context.Components
                .Include(c => c.ComponentActivities)
                .Include(c => c.PartComponentAPLs)
                    .ThenInclude(pca => pca.APL)
                        .ThenInclude(apl => apl.Parts)
                .Include(c => c.CustomParts)
                .Include(c => c.Historicals)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.QrToken == request.Token, cancellationToken);

            if (component == null)
                return null;

            // ============================
            // 2️⃣ Return DTO lengkap
            // ============================
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

                ComponentActivities = component.ComponentActivities
                    .Select(act => new ComponentActivityDto
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
