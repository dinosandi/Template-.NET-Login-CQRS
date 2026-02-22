using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
    public class PatchComponentCommand : IRequest<bool>
    {

        public DateTime? TanggalInstall { get; set; }
        public ComponentStatus? Status { get; set; }
        public string? NomerLaMbung { get; set; }
    }
    public class PatchComponentCommandWithId : PatchComponentCommand, IRequest<bool>
    {
        public Guid Id { get; set; }
    }

}
