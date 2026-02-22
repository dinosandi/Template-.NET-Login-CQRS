using MediatR;
using ComponentManagement.Application.Interfaces;

namespace ComponentManagement.Application.APLs.Queries
{
    public class GetAPLWithPartsQueryHandler : IRequestHandler<GetAPLWithPartsQuery, APLDto>
    {
        private readonly IAPLRepository _aplRepository;

        public GetAPLWithPartsQueryHandler(IAPLRepository aplRepository)
        {
            _aplRepository = aplRepository;
        }

        public async Task<APLDto> Handle(GetAPLWithPartsQuery request, CancellationToken cancellationToken)
        {
            var apl = await _aplRepository.GetByIdWithPartsAsync(request.Id);
            if (apl == null)
            {
                throw new KeyNotFoundException("APL not found");
            }

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
                    Priority = p.Priority,
                    Quantity = p.Quantity
                }).ToList()
            };
        }
    }
}
