using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.ComponentActivities.Queries;
using ComponentManagement.Application.Common.Dtos;
using ComponentManagement.Application.Interfaces;

namespace ComponentManagement.Application.ComponentActivities.Handlers
{
    public class GetComponentActivityByIdHandler : IRequestHandler<GetComponentActivityByIdQuery, GetComponentActivityByIdResponse>
    {
        private readonly IAppDbContext _context;

        public GetComponentActivityByIdHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<GetComponentActivityByIdResponse> Handle(GetComponentActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var activity = await _context.ComponentActivities
                .Include(a => a.Component)
                .FirstOrDefaultAsync(a => a.Id == request.ActivityId, cancellationToken);

            if (activity == null)
                return null!; // controller nanti akan handle 404

            return new GetComponentActivityByIdResponse
            {
                ActivityId = activity.Id,
                ComponentId = activity.ComponentId,
                DocumentationPath = activity.Documentation, // <-- gunakan Documentation dari entity
                NeedSupport = activity.NeedSupport,
                Status = activity.Status,
                CreatedAt = activity.CreatedAt,
                Component = new PartComponentDto
                {
                    Id = activity.Component.Id,
                    NamaBrand = activity.Component.NamaBrand,
                    NamaKomponen = activity.Component.NamaKomponen,
                    PartNumber = activity.Component.PartNumber,
                    NomerLaMbung = activity.Component.NomerLaMbung,
                    ImagePath = activity.Component.ImagePath,
                    Status = activity.Component.Status,
                    CreatedAt = activity.Component.CreatedAt,
                    UpdatedAt = activity.Component.UpdatedAt,
                    Note = activity.Component.Note
                }
            };
        }
    }
}
