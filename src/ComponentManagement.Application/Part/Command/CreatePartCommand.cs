using MediatR;

namespace ComponentManagement.Application.Parts.Commands
{
    public class CreatePartCommand : IRequest<CreatePartResponse>
    {
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }
    }

    public class CreatePartResponse
    {
        public Guid Id { get; set; }
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }
    }
}
