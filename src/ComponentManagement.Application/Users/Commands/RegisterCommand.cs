using ComponentManagement.Domain.Enums;
using MediatR;

namespace ComponentManagement.Application.Users.Commands
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string NoHp { get; set; }
        public required UserRole Role { get; set; }
    }

    public class RegisterResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
