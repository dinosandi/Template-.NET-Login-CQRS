using MediatR;
using ComponentManagement.Application.ComponentActivities.Commands;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.ComponentActivities.Handlers
{
    public class UpdateComponentActivityHandler 
        : IRequestHandler<UpdateComponentActivityCommand, UpdateComponentActivityResponse>
    {
        private readonly IComponentRepository _componentRepository;
        private readonly IComponentActivityRepository _activityRepository;
        private readonly IFileService _fileService;
        private readonly IWhatsappNotificationService _whatsapp;


        public UpdateComponentActivityHandler(
            IComponentRepository componentRepository,
            IComponentActivityRepository activityRepository,
            IFileService fileService,
            IWhatsappNotificationService whatsapp)
        {
            _componentRepository = componentRepository;
            _activityRepository = activityRepository;
            _fileService = fileService;
            _whatsapp = whatsapp;
        }

        public async Task<UpdateComponentActivityResponse> Handle(UpdateComponentActivityCommand request, CancellationToken cancellationToken)
        {
            // 1. Cari PartComponent
            var component = await _componentRepository.GetByIdAsync(request.ComponentId);
            if (component == null)
                throw new Exception("Component not found");

            // 2. Simpan file PDF jika ada
            string? docPath = null;
            if (request.Documentation != null)
            {
                using var ms = new MemoryStream();
                await request.Documentation.CopyToAsync(ms, cancellationToken);
                var fileData = ms.ToArray();

                docPath = await _fileService.SaveFileAsync(fileData, request.Documentation.FileName, cancellationToken);
            }

            // 3. Buat Activity baru
            var activity = new ComponentActivity
            {
                Id = Guid.NewGuid(),
                ComponentId = component.Id,
                Documentation = docPath,
                NeedSupport = request.NeedSupport,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _activityRepository.AddAsync(activity);

            // 4. Update status di PartComponent
            component.Status = request.Status;
            component.UpdatedAt = DateTime.UtcNow;
            await _componentRepository.UpdateAsync(component);

            // 5. Simpan perubahan
            await _componentRepository.SaveChangesAsync();

            // Kirim notifikasi WhatsApp jika status ada Fabrication Request
            if (request.Status == ComponentStatus.WIP3) // atau status FR kamu
{
    await _whatsapp.SendFabricationRequestQrAsync(component);
}

            // 6. Return response lengkap
            return new UpdateComponentActivityResponse
            {
                ActivityId = activity.Id,
                ComponentId = component.Id,
                DocumentationPath = docPath,
                Status = request.Status,
                CreatedAt = activity.CreatedAt,
                Component = new PartComponentDto
                {
                    Id = component.Id,
                    NamaBrand = component.NamaBrand,
                    NamaKomponen = component.NamaKomponen,
                    PartNumber = component.PartNumber,
                    NomerLaMbung = component.NomerLaMbung,
                    ImagePath = component.ImagePath,
                    Status = component.Status,
                    CreatedAt = component.CreatedAt,
                    UpdatedAt = component.UpdatedAt,
                    Note = component.Note
                }
            };
        }
    }
}
