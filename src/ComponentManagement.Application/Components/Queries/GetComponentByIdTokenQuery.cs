using MediatR;
using ComponentManagement.Application.Components.Dtos;

namespace ComponentManagement.Application.Components.Queries
{
    public class GetComponentByTokenQuery : IRequest<ComponentDto?>
    {
        public string Token { get; set; }

        public GetComponentByTokenQuery(string token)
        {
            Token = token;
        }
    }
}
