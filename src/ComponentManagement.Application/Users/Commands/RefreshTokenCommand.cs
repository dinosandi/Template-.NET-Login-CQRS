using MediatR;

namespace ComponentManagement.Application.Users.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResult>
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string? CreatedByIp { get; set; }
    }

    public class RefreshTokenResult
    {
        public string Token { get; set; } = string.Empty;
        public string NewRefreshToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        

    }
}
