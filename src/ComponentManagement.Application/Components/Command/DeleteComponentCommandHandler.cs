using MediatR;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Components.Commands
{
    public class DeleteComponentCommandHandler : IRequestHandler<DeleteComponentCommand, bool>
    {
        private readonly IAppDbContext _context;
        private readonly IFileService _fileService;

        public DeleteComponentCommandHandler(IAppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<bool> Handle(DeleteComponentCommand request, CancellationToken cancellationToken)
        {
            var component = await _context.Components
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (component == null) return false;

            // Hapus file fisik jika ada
            if (!string.IsNullOrEmpty(component.ImagePath))
            {
                await _fileService.DeleteFileAsync(component.ImagePath);
            }

            // Hapus data dari database
            _context.Components.Remove(component);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
