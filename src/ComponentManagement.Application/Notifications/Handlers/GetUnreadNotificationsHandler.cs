
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Application.Notifications.Dtos;
using ComponentManagement.Application.Notifications.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ComponentManagement.Application.Notifications.Handlers
{
    public class GetUnreadNotificationsHandler
    : IRequestHandler<GetUnreadNotificationsQuery, List<NotificationDto>>
    {
        private readonly IAppDbContext _context;

        public GetUnreadNotificationsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> Handle(
            GetUnreadNotificationsQuery request,
            CancellationToken ct)
        {
            return await _context.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    ComponentId = n.ComponentId,
                    Title = n.Title,
                    Message = n.Message,
                    NamaKomponen = n.NamaKomponen,
                    ImagePath = n.ImagePath,
                    Note = n.Note,
                    UnitName = n.UnitName,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(ct);
        }
    }

}


