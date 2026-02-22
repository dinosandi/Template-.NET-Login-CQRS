using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Notifications.Interfaces;
using ComponentManagement.Application.Components.Commands;
using ComponentManagement.Domain.Notifications;

namespace ComponentManagement.Application.Components.Handlers
{
    public class PatchComponentUnitCommandHandler
    : IRequestHandler<PatchComponentUnitCommandWithId, PatchComponentUnitResponse>
    {
        private readonly IComponentRepository _componentRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IWhatsappNotificationService _whatsappNotificationService;
        private readonly INotificationRepository _notificationRepository;

       public PatchComponentUnitCommandHandler(
            IComponentRepository componentRepository,
            IUnitRepository unitRepository,
            IWhatsappNotificationService whatsappNotificationService,
            INotificationRepository notificationRepository
        )
        {
            _componentRepository = componentRepository;
            _unitRepository = unitRepository;
            _whatsappNotificationService = whatsappNotificationService;
            _notificationRepository = notificationRepository;
        }

        public async Task<PatchComponentUnitResponse> Handle( PatchComponentUnitCommandWithId request, CancellationToken cancellationToken)
        {
            var component = await _componentRepository
                .GetByIdAsync(request.ComponentId);

            if (component is null)
                throw new InvalidOperationException("Component not found");

            var unit = await _unitRepository
                .GetByIdAsync(request.UnitId);

            if (unit is null)
                throw new InvalidOperationException("Unit not found");

            // =========================
            // DOMAIN LOGIC
            // =========================
            component.RequestInstallation(unit.Id, request.Note);
            

            // =========================
            // CREATE NOTIFICATION (CORE)
            // =========================
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                ComponentId = component.Id,
                Title = "INSTALLATION REQUEST",
                Message = $"Permintaan instalasi komponen {component.NamaKomponen}",
                NamaKomponen = component.NamaKomponen,
                ImagePath = component.ImagePath,
                Note = component.Note,
                UnitId = unit.Id,
                UnitName = unit.NameUnit,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _notificationRepository.AddAsync(notification, cancellationToken);
            await _componentRepository.SaveChangesAsync();

            // =========================
            // SIDE EFFECT
            // =========================
            await _whatsappNotificationService
                .SendInstallationRequestNotificationAsync(component, unit);

            return new PatchComponentUnitResponse
            {
                ComponentId = component.Id,
                UnitId = unit.Id,
                UnitName = unit.NameUnit,
                ComponentStatus = component.Status,
                UnitCreatedAt = unit.CreatedAt,
                UnitUpdatedAt = unit.UpdatedAt
            };
        }

    }
}
