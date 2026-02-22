using MediatR;

namespace ComponentManagement.Application.Parts.Queries
{
    public class GetAllPartQuery : IRequest<IEnumerable<PartDto>>
    {
    }

    public class PartDto
    {
        public Guid Id { get; set; }
        public string NamaPart { get; set; }
        public string PartNumber { get; set; }
    }
}
