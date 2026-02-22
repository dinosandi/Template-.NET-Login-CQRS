using MediatR;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Components.Commands;
using ComponentManagement.Domain.Entities;
using QRCoder;

public class CreateComponentCommandHandler : IRequestHandler<CreateComponentCommand, CreateComponentResponse>
{
    private readonly IComponentRepository _componentRepository;
    private readonly IPartRepository _partRepository;
    private readonly IFileService _fileService;
    private readonly ITokenService _tokenService;
    private readonly IWhatsappNotificationService _waService;


    public CreateComponentCommandHandler(
        IComponentRepository componentRepository,
        IPartRepository partRepository,
        IFileService fileService,
        ITokenService tokenService,
        IWhatsappNotificationService waService)
    {
        _componentRepository = componentRepository;
        _partRepository = partRepository;
        _fileService = fileService;
        _tokenService = tokenService;
        _waService = waService;

    }

    public async Task<CreateComponentResponse> Handle(
        CreateComponentCommand request,
        CancellationToken cancellationToken)
    {
        string? imagePath = null;

        // ============================
        // 1. Save component image
        // ============================
        if (request.FileData != null && request.FileName != null)
        {
            imagePath = await _fileService.SaveFileAsync(
                request.FileData,
                request.FileName,
                cancellationToken
            );
        }

        // ============================
        // 2. Validate Part
        // ============================
        if (request.PartId.HasValue)
        {
            var part = await _partRepository.GetByIdAsync(request.PartId.Value);
            if (part == null)
                throw new ArgumentException("Part tidak ditemukan");
        }

        // ============================
        // 3. Create Component
        // ============================
        var component = new PartComponent
        {
            Id = Guid.NewGuid(),
            PartId = request.PartId,
            NamaBrand = request.NamaBrand,
            NamaKomponen = request.NamaKomponen,
            PartNumber = request.PartNumber,
            NomerLaMbung = request.NomerLaMbung,
            TanggalInstall = request.TanggalInstall,
            Note = request.Note,
            Status = request.Status,
            ImagePath = imagePath,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _componentRepository.AddAsync(component);
        await _componentRepository.SaveChangesAsync();

        // ============================
        // 4. Generate QR Token
        // ============================
        string qrToken = Guid.NewGuid().ToString();
        component.QrToken = qrToken; 

        // ============================
        // 5. Generate QR PNG + Base64
        // ============================
        var payload = $"http://localhost:5173/Component/{qrToken}";

        string base64;
        byte[] pngBytes;

        using (var generator = new QRCodeGenerator())
        {
            var data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            var base64Qr = new Base64QRCode(data);
            base64 = "data:image/png;base64," + base64Qr.GetGraphic(20);

            var pngQr = new PngByteQRCode(data);
            pngBytes = pngQr.GetGraphic(20);
        }

        component.QrBase64 = base64;

        // ============================
        // 6. Save QR PNG (file)
        // ============================
        var qrFileName = $"qr_{component.Id}.png";
        var qrImageUrl = await _fileService.SaveFileAsync(
            pngBytes,
            qrFileName,
            cancellationToken
        );

        component.QrImageUrl = qrImageUrl;

        // ============================
        // 7. Update component
        // ============================
        await _componentRepository.UpdateAsync(component);
        await _componentRepository.SaveChangesAsync();

        try
        {
            await _waService.SendComponentStatusNotificationAsync(component);
        }
        catch (Exception ex)
        {
            // Log error tapi jangan ganggu proses utama
            Console.WriteLine($"Failed to send WhatsApp notification: {ex.Message}");
        }



        // ============================
        // 8. Prepare response
        // ============================
        return new CreateComponentResponse
        {
            Id = component.Id,
            PartId = component.PartId,
            NamaBrand = component.NamaBrand,
            NamaKomponen = component.NamaKomponen,
            PartNumber = component.PartNumber,
            NomerLaMbung = component.NomerLaMbung,
            TanggalInstall = component.TanggalInstall,
            Status = component.Status,
            Note = component.Note,
            ImagePath = component.ImagePath,
            QrBase64 = component.QrBase64,
            QrImageUrl = component.QrImageUrl,
            QrToken = component.QrToken,
            CreatedAt = component.CreatedAt,
            UpdatedAt = component.UpdatedAt
        };
    }
}
