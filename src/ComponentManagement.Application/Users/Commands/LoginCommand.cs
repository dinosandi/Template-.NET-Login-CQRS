using MediatR;

namespace ComponentManagement.Application.Users.Commands
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string RefreshToken { get; set; }
    }
}
