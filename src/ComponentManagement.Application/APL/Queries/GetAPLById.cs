using MediatR;

namespace ComponentManagement.Application.APLs.Queries
{
    public class GetAPLWithPartsQuery : IRequest<APLDto>
    {
        public Guid Id { get; set; }

        public GetAPLWithPartsQuery(Guid id)
        {
            Id = id;
        }
    }
}
