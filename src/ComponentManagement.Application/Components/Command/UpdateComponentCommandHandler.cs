using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Components.Commands
{
    public class UpdateComponentCommandHandler : IRequestHandler<UpdateComponentCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IFileService _fileService;

        public UpdateComponentCommandHandler(IAppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<bool> Handle(UpdateComponentCommand request, CancellationToken cancellationToken)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (component == null) return false;

            component.NamaBrand = request.NamaBrand;
            component.NamaKomponen = request.NamaKomponen;
            component.PartNumber = request.PartNumber;
            component.NomerLaMbung = request.NomerLaMbung;
            component.Note = request.Note;
            component.Status = request.Status;
            component.UpdatedAt = DateTime.UtcNow;

            if (request.FileData != null && request.FileName != null)
            {
                component.ImagePath = await _fileService.SaveFileAsync(
                    request.FileData, request.FileName, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
