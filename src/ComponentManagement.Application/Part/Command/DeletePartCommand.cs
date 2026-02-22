using MediatR;

namespace ComponentManagement.Application.Parts.Commands
{
    public class DeletePartCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
