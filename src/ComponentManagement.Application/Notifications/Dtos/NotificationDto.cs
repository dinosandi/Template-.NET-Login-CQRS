using System;

namespace ComponentManagement.Application.Notifications.Dtos
{
    public class NotificationDto
{
    public Guid Id { get; set; }
    public Guid ComponentId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string NamaKomponen { get; set; } = null!;
    public string? ImagePath { get; set; }
    public string? Note { get; set; }
    public string? UnitName { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}


}
