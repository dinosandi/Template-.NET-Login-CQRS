using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Parts.Commands
{
    public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, CreatePartResponse>
    {
        private readonly IPartRepository _partRepository;

        public CreatePartCommandHandler(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<CreatePartResponse> Handle(CreatePartCommand request, CancellationToken cancellationToken)
        {
            var part = new Part
            {
                Id = Guid.NewGuid(),
                NamaPart = request.NamaPart,
                PartNumber = request.PartNumber
            };

            await _partRepository.AddAsync(part);
            await _partRepository.SaveChangesAsync();

            return new CreatePartResponse
            {
                Id = part.Id,
                NamaPart = part.NamaPart,
                PartNumber = part.PartNumber
            };
        }
    }
}
