using MediatR;
using ComponentManagement.Application.Interfaces;

namespace ComponentManagement.Application.Parts.Commands
{
    public class DeletePartCommandHandler : IRequestHandler<DeletePartCommand, bool>
    {
        private readonly IPartRepository _partRepository;

        public DeletePartCommandHandler(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task<bool> Handle(DeletePartCommand request, CancellationToken cancellationToken)
        {
            var part = await _partRepository.GetByIdAsync(request.Id);
            if (part == null)
            {
                return false;
            }

            _partRepository.Delete(part);
            await _partRepository.SaveChangesAsync();

            return true;
        }
    }
}
