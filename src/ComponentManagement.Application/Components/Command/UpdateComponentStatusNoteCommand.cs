using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class UpdateComponentStatusNoteCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ComponentStatus? Status { get; set; }
        public string? Note { get; set; }
    }
}
