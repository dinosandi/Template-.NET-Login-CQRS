using MediatR;
using Microsoft.Extensions.Configuration;
using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Users.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IAuthenticationService _authService;
        private readonly IConfiguration _config;

        public RefreshTokenCommandHandler(
            IRefreshTokenService refreshTokenService,
            IAuthenticationService authService,
            IConfiguration config)
        {
            _refreshTokenService = refreshTokenService;
            _authService = authService;
            _config = config;
        }

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // 🔹 Ambil token valid dari database
            var tokenEntity = await _refreshTokenService.GetValidTokenAsync(request.RefreshToken);
            if (tokenEntity == null)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = tokenEntity.User;
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // 🔹 Revoke token lama
            await _refreshTokenService.RevokeTokenAsync(tokenEntity, request.CreatedByIp ?? "unknown", "Token rotated");

            // 🔹 Generate access token baru
            var newAccessToken = _authService.GenerateJwtToken(user);

            // 🔹 Buat refresh token baru (gunakan tipe eksplisit)
            (string rawRefreshToken, RefreshToken newRefreshTokenEntity) =
                await _refreshTokenService.CreateNewTokenAsync(user, request.CreatedByIp ?? "unknown");

            // 🔹 Kembalikan hasil
            return new RefreshTokenResult
            {
                Token = newAccessToken,
                NewRefreshToken = rawRefreshToken,
                Role = user.Role.ToString()
            };
        }
    }
}
