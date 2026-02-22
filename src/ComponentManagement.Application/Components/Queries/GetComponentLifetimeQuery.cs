using ComponentManagement.Application.Components.Dtos;
using MediatR;

namespace ComponentManagement.Application.Components.Queries
{
    public class GetComponentLifetimeQuery : IRequest<ComponentLifetimeDto>
    {
        public Guid ComponentId { get; set; }
    }
}
