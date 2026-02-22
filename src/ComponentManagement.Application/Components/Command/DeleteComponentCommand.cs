using MediatR;

namespace ComponentManagement.Application.Components.Commands
{
    public class DeleteComponentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteComponentCommand(Guid id)
        {
            Id = id;
        }
    }
}
