using ComponentManagement.Domain.Entities;


namespace ComponentManagement.Application.Interfaces
{
    public interface IWhatsappNotificationService
    {
        Task SendComponentStatusNotificationAsync(PartComponent component);
        Task SendComponentNeedAplNotificationAsync(PartComponent component);
        Task SendFabricationRequestQrAsync(PartComponent component);
        Task SendUpdateStatusComponentAsync(PartComponent component);
        Task SendInstallationRequestNotificationAsync(PartComponent component, Unit unit);
}
}

