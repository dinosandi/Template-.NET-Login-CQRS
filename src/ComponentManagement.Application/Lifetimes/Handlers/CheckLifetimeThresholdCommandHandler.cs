using MediatR;
using Microsoft.EntityFrameworkCore;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Lifetimes.Commands;
using ComponentManagement.Domain.ValueObjects;

public sealed class CheckLifetimeThresholdCommandHandler
    : IRequestHandler<CheckLifetimeThresholdCommand>
{
    private readonly IAppDbContext _context;
    private readonly ILifetimeNotificationService _notification;

    private static readonly int[] Thresholds = LifetimeThreshold.Values;

    public CheckLifetimeThresholdCommandHandler(
        IAppDbContext context,
        ILifetimeNotificationService notification)
    {
        _context = context;
        _notification = notification;
    }

    public async Task Handle(
        CheckLifetimeThresholdCommand request,
        CancellationToken cancellationToken)
    {
        // =========================
        // 1. Ambil waktu SEKALI (UTC)
        // =========================
        var nowUtc = DateTime.UtcNow;

        // =========================
        // 2. Ambil lifetime aktif
        // =========================
        var lifetimes = await _context.ComponentLifetimes
            .Include(x => x.PartComponent)
                .ThenInclude(pc => pc.Unit)
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var lifetime in lifetimes)
        {
            // =========================
            // 3. Hitung remaining HM
            // =========================
            var remainingHm = lifetime.CalculateRemainingHm(nowUtc);

            // =========================
            // 4. Cek threshold (besar → kecil)
            // =========================
            foreach (var threshold in Thresholds.OrderByDescending(x => x))
            {
                if (remainingHm > threshold)
                    continue;

                if (lifetime.HasNotified(threshold))
                    continue;

                // =========================
                // 5. Kirim notifikasi
                // =========================
                await _notification.SendLifetimeWarningAsync(
                    lifetime,
                    threshold,
                    cancellationToken);

                // =========================
                // 6. Tandai sudah notifikasi
                // =========================
                lifetime.MarkNotified(threshold);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
