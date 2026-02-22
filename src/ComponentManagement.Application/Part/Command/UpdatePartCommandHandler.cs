using MediatR;
using ComponentManagement.Application.Interfaces;

namespace ComponentManagement.Application.Parts.Commands
{
    public class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand, UpdatePartResponse>
    {
        private readonly IPartRepository _partRepository;

        public UpdatePartCommandHandler(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<UpdatePartResponse> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
        {
            var part = await _partRepository.GetByIdAsync(request.Id);
            if (part == null)
            {
                throw new KeyNotFoundException($"Part dengan ID {request.Id} tidak ditemukan.");
            }

            part.NamaPart = request.NamaPart;
            part.PartNumber = request.PartNumber;

            await _partRepository.SaveChangesAsync();

            return new UpdatePartResponse
            {
                Id = part.Id,
                NamaPart = part.NamaPart,
                PartNumber = part.PartNumber
            };
        }
    }
}
