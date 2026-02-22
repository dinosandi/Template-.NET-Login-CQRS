using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Units.Commands
{
    public class UpdateUnitCommand : IRequest<UpdateUnitResponse>
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ComponentStatus Status { get; set; }
    }

    public class UpdateUnitResponse
    {
        public Guid Id { get; set; }
        public string NameUnit { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ComponentStatus Status { get; set; }
    }
}
