using MediatR;

namespace ComponentManagement.Application.Parts.Commands
{
    public class UpdatePartCommand : IRequest<UpdatePartResponse>
    {
        public Guid Id { get; set; }
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }
    }

    public class UpdatePartResponse
    {
        public Guid Id { get; set; }
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }
    }
}
