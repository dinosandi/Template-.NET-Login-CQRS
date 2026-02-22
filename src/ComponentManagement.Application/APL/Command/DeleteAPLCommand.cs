using MediatR;

namespace ComponentManagement.Application.APLs.Commands
{
    public class DeleteAPLCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteAPLCommand(Guid id)
        {
            Id = id;
        }
    }
}
