using MediatR;
using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentManagement.Application.Users.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;

        public LoginCommandHandler(IUserService userService, IAuthenticationService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
                throw new UnauthorizedException("Invalid username or password");

            if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid username or password");

            var token = _authService.GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}
