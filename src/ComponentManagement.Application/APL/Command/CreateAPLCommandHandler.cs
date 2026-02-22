using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.APLs.Commands
{
    public class CreateAPLCommandHandler : IRequestHandler<CreateAPLCommand, CreateAPLResponse>
    {
        private readonly IAPLRepository _aplRepository;
        private readonly IAPLPartRepository _aplPartRepository;

        public CreateAPLCommandHandler(IAPLRepository aplRepository, IAPLPartRepository aplPartRepository)
        {
            _aplRepository = aplRepository;
            _aplPartRepository = aplPartRepository;
        }

        public async Task<CreateAPLResponse> Handle(CreateAPLCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var apl = new APL
            {
                Id = Guid.NewGuid(),
                NameBrand = request.NameBrand,
                CreatedAt = now,
                UpdatedAt = now,
            };

            // Tambahkan parts ke entity APL
            foreach (var part in request.Parts)
            {
                apl.Parts.Add(new APLPart
                {
                    Id = Guid.NewGuid(),
                    PartNumber = part.PartNumber,
                    Description = part.Description,
                    Quantity = part.Quantity,
                    CreatedAt = now,
                    UpdatedAt = now,
                    APLId = apl.Id
                });
            }

            // Simpan via repository
            await _aplRepository.AddAsync(apl);
            await _aplRepository.SaveChangesAsync();

            return new CreateAPLResponse
            {
                Id = apl.Id,
                NameBrand = apl.NameBrand,
                CreatedAt = apl.CreatedAt
            };
        }
    }
}
