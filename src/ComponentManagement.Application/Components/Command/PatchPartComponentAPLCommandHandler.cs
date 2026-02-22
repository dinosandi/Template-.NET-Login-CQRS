using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchPartComponentAPLCommandHandler
        : IRequestHandler<PatchPartComponentAPLCommand, PatchPartComponentAPLResponse>
    {
        private readonly IComponentRepository _partComponentRepository;
        private readonly IAPLRepository _aplRepository;
        private readonly IPartComponentAPLRepository _partComponentAplRepository;
        private readonly IComponentCustomPartRepository _customPartRepository;
        private readonly IWhatsappNotificationService _whatsappNotificationService;

        public PatchPartComponentAPLCommandHandler(
            IComponentRepository partComponentRepository,
            IAPLRepository aplRepository,
            IPartComponentAPLRepository partComponentAplRepository,
            IComponentCustomPartRepository customPartRepository,
            IWhatsappNotificationService whatsappNotificationService)
        {
            _partComponentRepository = partComponentRepository;
            _aplRepository = aplRepository;
            _partComponentAplRepository = partComponentAplRepository;
            _customPartRepository = customPartRepository;
            _whatsappNotificationService = whatsappNotificationService;
        }

        public async Task<PatchPartComponentAPLResponse> Handle(
            PatchPartComponentAPLCommand request,
            CancellationToken cancellationToken)
        {
            // ✅ Pastikan PartComponent ada
            var partComponent = await _partComponentRepository.GetByIdAsync(request.PartComponentId);
            if (partComponent == null)
                throw new Exception("PartComponent not found");

            // ✅ Ambil APL berdasarkan NameBrand (tidak boleh bikin baru)
            var apl = (await _aplRepository.GetAllAsync())
                .FirstOrDefault(x => x.NameBrand == request.NameBrand);

            if (apl == null)
                throw new Exception("APL not found");

            // ✅ Pastikan relasi PartComponent ↔ APL ada, kalau belum buat
            var existingRelation = (await _partComponentAplRepository.GetAllAsync())
                .FirstOrDefault(r => r.PartComponentId == partComponent.Id && r.APLId == apl.Id);

            if (existingRelation == null)
            {
                var newRelation = new PartComponentAPL
                {
                    Id = Guid.NewGuid(),
                    PartComponentId = partComponent.Id,
                    APLId = apl.Id
                };
                await _partComponentAplRepository.AddAsync(newRelation);
                var partComponentWithApls =
                await _partComponentRepository.GetByIdWithAplsAsync(partComponent.Id);
                partComponent.Status = ComponentStatus.WIP2;
                await _partComponentRepository.UpdateAsync(partComponent);
                await _whatsappNotificationService.SendComponentNeedAplNotificationAsync(partComponentWithApls);
            }

            // ✅ Jika user menambahkan custom parts (opsional)
            if (request.ComponentCustomParts != null && request.ComponentCustomParts.Any())
            {
                foreach (var custom in request.ComponentCustomParts)
                {
                    var newCustom = new ComponentCustomPart
                    {
                        Id = Guid.NewGuid(),
                        PartComponentId = partComponent.Id,
                        NameBrand = custom.NameBrand,
                        PartNumber = custom.PartNumber,
                        Description = custom.Description,
                        Priority = custom.Priority,
                        Quantity = custom.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _customPartRepository.AddAsync(newCustom);
                }
            }

            await _aplRepository.SaveChangesAsync();

            var allApls = await _aplRepository.GetAllAsync();

            return new PatchPartComponentAPLResponse
            {
                PartComponentId = partComponent.Id,
                UpdatedAPL = MapToDto(apl),
                AllAPLs = allApls.Select(MapToDto).ToList()
            };
        }

        private static APLDto MapToDto(APL apl)
        {
            return new APLDto
            {
                Id = apl.Id,
                NameBrand = apl.NameBrand,
                CreatedAt = apl.CreatedAt,
                UpdatedAt = apl.UpdatedAt,
                Parts = apl.Parts.Select(p => new APLPartDto
                {
                    Id = p.Id,
                    PartNumber = p.PartNumber,
                    Description = p.Description,
                    Quantity = p.Quantity,
                    Priority = p.Priority
                }).ToList()
            };
        }
    }
}
