using MediatR;
using ComponentManagement.Domain.Enums;

namespace ComponentManagement.Application.Components.Commands
{
   public class PatchComponentUnitCommandWithId
    : IRequest<PatchComponentUnitResponse>
{
    public Guid ComponentId { get; set; }
    public Guid UnitId { get; set; }
    public string? Note { get; set; }
}


    public class PatchComponentUnitResponse
    {
        public Guid ComponentId { get; set; }
        public Guid UnitId { get; set; }
        public string UnitName { get; set; } = default!;
        public DateTime UnitCreatedAt { get; set; }
        public DateTime UnitUpdatedAt { get; set; }
        public ComponentStatus ComponentStatus { get; set; }
    }
    public class PatchComponentUnitRequest
{
    public Guid UnitId { get; set; }
    public string? Note { get; set; }
}

}
