using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Components.Dtos;
using ComponentManagement.Application.Components.Queries;
using ComponentManagement.Application.Exceptions;
using ComponentManagement.Application.Common;

public class GetComponentLifetimeQueryHandler
    : IRequestHandler<GetComponentLifetimeQuery, ComponentLifetimeDto>
{
    private readonly IAppDbContext _context;

    public GetComponentLifetimeQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ComponentLifetimeDto> Handle(
        GetComponentLifetimeQuery request,
        CancellationToken cancellationToken)
    {
        var lifetime = await _context.ComponentLifetimes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.PartComponentId == request.ComponentId &&
                x.IsActive,
                cancellationToken);

        if (lifetime == null)
            throw new BadRequestException("Active lifetime not found.");

        var now = DateTime.UtcNow;

        var usedHm = LifetimeCalculator.CalculateUsedHm(lifetime.InstalledAt.Value);
        var remainingHm = LifetimeCalculator.CalculateRemainingHm(
            lifetime.TotalLifetimeHm,
            lifetime.InstalledAt.Value
        );


        return new ComponentLifetimeDto
        {
            TotalLifetimeHm = lifetime.TotalLifetimeHm,
            UsedLifetimeHm = usedHm,
            RemainingLifetimeHm = remainingHm,
            InstalledAt = lifetime.InstalledAt
        };
    }
}
