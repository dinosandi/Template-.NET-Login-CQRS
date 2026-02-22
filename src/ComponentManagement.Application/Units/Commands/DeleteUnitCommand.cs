using MediatR;

namespace ComponentManagement.Application.Units.Commands
{
    public class DeleteUnitCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteUnitCommand(Guid id)
        {
            Id = id;
        }
    }
}
