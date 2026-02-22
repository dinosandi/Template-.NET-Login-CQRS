using MediatR;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Exceptions;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchHistoricalComponentHandler
        : IRequestHandler<PatchHistoricalCommand, PatchHistoricalResponse>
    {
        private readonly IComponentRepository _componentRepository;
        private readonly IHistoricalRepository _historicalRepository;
        private readonly IComponentLifetimeRepository _lifetimeRepository;

        public PatchHistoricalComponentHandler(
            IComponentRepository componentRepository,
            IHistoricalRepository historicalRepository,
            IComponentLifetimeRepository lifetimeRepository)
        {
            _componentRepository = componentRepository;
            _historicalRepository = historicalRepository;
            _lifetimeRepository = lifetimeRepository;
        }

        public async Task<PatchHistoricalResponse> Handle(
            PatchHistoricalCommand request,
            CancellationToken cancellationToken)
        {
            // =========================
            // GET COMPONENT + HISTORICAL
            // =========================
            var partComponent =
                await _componentRepository.GetByIdWithHistoricalsAsync(request.PartComponentId);

            if (partComponent == null)
                throw new BadRequestException("PartComponent not found.");

            // =========================
            // VALIDATION (DOMAIN RULE)
            // =========================

            // Historical hanya boleh dibuat saat INSTALLED
            if (partComponent.Status != ComponentStatus.INSTALLED)
                throw new BadRequestException(
                    "Historical can only be created when component is INSTALLED.");

            // Max 3 historical
            if (partComponent.Historicals.Count >= 3)
                throw new HistoricalLimitException();

            // Component harus punya tanggal install
            if (!partComponent.TanggalInstall.HasValue)
                throw new BadRequestException("Component has not been installed yet.");
            // =========================
            // CALCULATE HM (AUTO)
            // =========================
            var tanggalInstall = partComponent.TanggalInstall.Value;
            var tanggalRusak = request.TanggalRFU ?? DateTimeOffset.UtcNow;

            if (tanggalRusak < tanggalInstall)
                throw new BadRequestException("Invalid RFU date.");

            var hmInHours = Math.Round(
                (tanggalRusak - tanggalInstall).TotalHours, 2);

            // =========================
            // STOP ACTIVE LIFETIME
            // =========================
            var activeLifetime =
                await _lifetimeRepository.GetActiveByComponentIdAsync(partComponent.Id);

            if (activeLifetime != null)
            {
                activeLifetime.UsedLifetimeHm = hmInHours;
                activeLifetime.RemainingLifetimeHm =
                    Math.Max(0, activeLifetime.TotalLifetimeHm - hmInHours);

                activeLifetime.IsActive = false;
                activeLifetime.UpdatedAt = DateTime.UtcNow;
            }

            // =========================
            // CREATE HISTORICAL
            // =========================
            var newHistorical = new Historical
            {
                Id = Guid.NewGuid(),
                PartComponentId = partComponent.Id,

                TanggalInstall = tanggalInstall,
                TanggalRFU = tanggalRusak,

                OldCodeNumber = partComponent.NomerLaMbung,
                NewCodeNumber = request.NewCodeNumber,

                Hm = hmInHours.ToString("0.00"),
                Action = request.Action,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // =========================
            // UPDATE COMPONENT (RFU)
            // =========================
            if (!string.IsNullOrWhiteSpace(request.NewCodeNumber))
            {
                partComponent.NomerLaMbung = request.NewCodeNumber;
            }

            partComponent.Status = ComponentStatus.RFU;
            partComponent.TanggalInstall = null; // stop HM
            partComponent.UpdatedAt = DateTime.UtcNow;

            // =========================
            // SAVE (ONE TRANSACTION)
            // =========================
            await _historicalRepository.AddAsync(newHistorical);
            await _lifetimeRepository.SaveChangesAsync();

            // =========================
            // RESPONSE
            // =========================
            return new PatchHistoricalResponse
            {
                PartComponentId = partComponent.Id,
                Message = "Historical record successfully added.",
                TotalHistorical = partComponent.Historicals.Count + 1,
                LatestHistorical = new HistoricalDto
                {
                    Id = newHistorical.Id,
                    TanggalInstall = newHistorical.TanggalInstall,
                    TanggalRFU = newHistorical.TanggalRFU,
                    OldCodeNumber = newHistorical.OldCodeNumber,
                    NewCodeNumber = newHistorical.NewCodeNumber,
                    Hm = newHistorical.Hm,
                    Action = newHistorical.Action,
                    CreatedAt = newHistorical.CreatedAt,
                    UpdatedAt = newHistorical.UpdatedAt
                }
            };
        }
    }
}
