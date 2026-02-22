using MediatR;
using ComponentManagement.Application.Components.Dtos;

namespace ComponentManagement.Application.Components.Queries
{
    public class GetComponentByIdQuery : IRequest<ComponentDto?>
    {
        public Guid Id { get; set; }

        public GetComponentByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
