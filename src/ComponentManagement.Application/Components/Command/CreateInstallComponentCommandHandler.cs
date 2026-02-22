using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Exceptions;
using ComponentManagement.Application.Components.Commands; // Ensure this is the correct namespace for InstallComponentCommand

public class InstallComponentCommandHandler
    : IRequestHandler<InstallComponentCommand, Guid>
{
    private readonly IAppDbContext _context;

    public InstallComponentCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        InstallComponentCommand request,
        CancellationToken cancellationToken)
    {
        var component = await _context.Components
            .FirstOrDefaultAsync(c => c.Id == request.PartComponentId, cancellationToken);

        if (component == null)
            throw new BadRequestException("Component not found.");

        // ❌ Tidak boleh install jika masih INSTALLED
        if (component.Status == ComponentStatus.INSTALLED)
            throw new BadRequestException("Component already installed.");

        // 🔹 Matikan lifetime lama (jika ada)
        var activeLifetime = await _context.ComponentLifetimes
            .FirstOrDefaultAsync(l =>
                l.PartComponentId == component.Id &&
                l.IsActive,
                cancellationToken);

        if (activeLifetime != null)
        {
            activeLifetime.IsActive = false;
            activeLifetime.UpdatedAt = DateTime.UtcNow;
        }

        // 🔥 KONVERSI WAKTU USER → UTC
     var installedAtUtc = request.InstalledAt.UtcDateTime;

        // PROTEKSI FUTURE TIME
        if (installedAtUtc > DateTime.UtcNow.AddMinutes(5))
            throw new BadRequestException("Installed time cannot be in the future.");

        component.Install(installedAtUtc);

        // 🔹 CREATE LIFETIME BARU
        var lifetime = new ComponentLifetime
        {
            Id = Guid.NewGuid(),
            PartComponentId = component.Id,

            TotalLifetimeHm = request.TotalLifetimeHm,
            InstalledAt = installedAtUtc,

            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ComponentLifetimes.Add(lifetime);
        await _context.SaveChangesAsync(cancellationToken);

        return lifetime.Id;
    }
}