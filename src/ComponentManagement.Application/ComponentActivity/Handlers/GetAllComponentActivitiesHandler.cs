using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.ComponentActivities.Queries;
using ComponentManagement.Application.Common.Dtos;
using ComponentManagement.Application.Interfaces;
using System.Linq;

namespace ComponentManagement.Application.ComponentActivities.Handlers
{
    public class GetAllComponentActivitiesHandler : IRequestHandler<GetAllComponentActivitiesQuery, GetAllComponentActivitiesResponse>
    {
        private readonly IAppDbContext _context;

        public GetAllComponentActivitiesHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllComponentActivitiesResponse> Handle(GetAllComponentActivitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ComponentActivities
                .Include(a => a.Component)
                .AsQueryable();

            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(a =>
                    a.Component.NamaKomponen.Contains(search) ||
                    a.Component.PartNumber.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var resultItems = items.Select(a => new ComponentActivityItemDto
            {
                ActivityId = a.Id,
                ComponentId = a.ComponentId,
                DocumentationPath = a.Documentation,
                NeedSupport = a.NeedSupport,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                Component = new PartComponentDto
                {
                    Id = a.Component.Id,
                    NamaBrand = a.Component.NamaBrand,
                    NamaKomponen = a.Component.NamaKomponen,
                    PartNumber = a.Component.PartNumber,
                    NomerLaMbung = a.Component.NomerLaMbung,
                    ImagePath = a.Component.ImagePath,
                    Status = a.Component.Status,
                    CreatedAt = a.Component.CreatedAt,
                    UpdatedAt = a.Component.UpdatedAt,
                    Note = a.Component.Note
                }
            }).ToList();

            return new GetAllComponentActivitiesResponse
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = resultItems
            };
        }
    }
}
