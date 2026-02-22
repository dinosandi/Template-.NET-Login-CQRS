using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.APLs.Commands
{
    public class UpdateAPLCommandHandler : IRequestHandler<UpdateAPLCommand, UpdateAPLResponse>
    {
        private readonly IAPLRepository _aplRepository;
        private readonly IAPLPartRepository _aplPartRepository;

        public UpdateAPLCommandHandler(IAPLRepository aplRepository, IAPLPartRepository aplPartRepository)
        {
            _aplRepository = aplRepository;
            _aplPartRepository = aplPartRepository;
        }

        public async Task<UpdateAPLResponse> Handle(UpdateAPLCommand request, CancellationToken cancellationToken)
        {
            var apl = await _aplRepository.GetByIdWithPartsAsync(request.Id);
            if (apl == null)
                throw new Exception("APL not found");

            // 🔹 Update APL utama
            apl.NameBrand = request.NameBrand;
            apl.UpdatedAt = DateTime.UtcNow;

            if (request.Parts != null && request.Parts.Any())
            {
                foreach (var partReq in request.Parts)
                {
                    // cari apakah part sudah ada
                    var part = apl.Parts.FirstOrDefault(p => p.Id == partReq.Id);

                    if (part != null)
                    {
                        if (partReq.IsDeleted) // kalau ada flag hapus
                        {
                            // benar-benar hapus dari database
                            _aplPartRepository.Delete(part);
                        }
                        else
                        {
                            // update data part
                            part.PartNumber = partReq.PartNumber ?? part.PartNumber;
                            part.Description = partReq.Description ?? part.Description;
                            part.Quantity = partReq.Quantity;
                            part.UpdatedAt = DateTime.UtcNow;

                            _aplPartRepository.Update(part);
                        }
                    }
                }
            }

            await _aplRepository.SaveChangesAsync();

            // 🔹 Return response
            return new UpdateAPLResponse
            {
                Id = apl.Id,
                NameBrand = apl.NameBrand,
                UpdatedAt = apl.UpdatedAt,
                Parts = apl.Parts.Select(p => new UpdateAPLPartResponse
                {
                    Id = p.Id,
                    PartNumber = p.PartNumber,
                    Description = p.Description,
                    Quantity = p.Quantity
                }).ToList()
            };
        }
    }
}
