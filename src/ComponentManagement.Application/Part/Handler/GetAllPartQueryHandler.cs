using MediatR;
using ComponentManagement.Application.Interfaces;

namespace ComponentManagement.Application.Parts.Queries
{
    public class GetAllPartQueryHandler : IRequestHandler<GetAllPartQuery, IEnumerable<PartDto>>
    {
        private readonly IPartRepository _partRepository;

        public GetAllPartQueryHandler(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<IEnumerable<PartDto>> Handle(GetAllPartQuery request, CancellationToken cancellationToken)
        {
            var parts = await _partRepository.GetAllAsync();

            return parts.Select(p => new PartDto
            {
                Id = p.Id,
                NamaPart = p.NamaPart,
                PartNumber = p.PartNumber
            });
        }
    }
}
