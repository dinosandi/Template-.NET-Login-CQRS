using MediatR;
using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Interfaces;


namespace ComponentManagement.Application.Users.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(IUserService userService, IAuthenticationService authService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _authService = authService;
            _refreshTokenService = refreshTokenService;
        }


        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
                throw new UnauthorizedException("Invalid username or password");

            if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid username or password");

            var token = _authService.GenerateJwtToken(user);

            // Buat refresh token via service
            var refreshToken = await _refreshTokenService.CreateNewTokenAsync(user);

            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role.ToString(),
                RefreshToken = refreshToken.RawToken
            };
        }

    }
}
