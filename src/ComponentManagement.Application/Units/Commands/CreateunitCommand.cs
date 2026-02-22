using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Units.Commands
{
    public class CreateUnitCommand : IRequest<CreateUnitResponse>
    {
        public string NameUnit { get; set; } = default!;
        // public string Description { get; set; } = default!;
        // public ComponentStatus Status { get; set; } = ComponentStatus.Requested;

    }
    public class CreateUnitResponse
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        // public string Description { get; set; } = default!;
        // public ComponentStatus Status { get; set; }
    }
}
