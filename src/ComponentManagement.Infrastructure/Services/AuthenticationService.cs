using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ComponentManagement.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly ITokenRepository _tokenRepo;
        private readonly IUserService _userService;

        public AuthenticationService(IConfiguration config, ITokenRepository tokenRepo, IUserService userService)
        {
            _config = config;
            _tokenRepo = tokenRepo;
            _userService = userService;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        // =============================
        // 🔹 Generate JWT Access Token
        // =============================
        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenMinutes"] ?? "15")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // =============================
        // 🔹 Generate Refresh Token
        // =============================
        public (string Token, RefreshToken Entity) GenerateRefreshToken(User user, string createdByIp)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
            {
                TokenHash = ComputeHash(rawToken),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenDays"] ?? "7")),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = createdByIp
            };

            return (rawToken, refreshToken);
        }

        // =============================
        // 🔹 Validate Refresh Token
        // =============================
        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string rawToken)
        {
            var hash = ComputeHash(rawToken);
            var dbToken = await _tokenRepo.GetRefreshTokenByHashAsync(hash);

            if (dbToken == null || !dbToken.IsActive || dbToken.ExpiresAt <= DateTime.UtcNow)
                return null;

            return dbToken;
        }

        // =============================
        // 🔹 Rotate Refresh Token
        // =============================
        public async Task RotateRefreshTokenAsync(RefreshToken currentToken, string newRawToken, string ipAddress)
        {
            var newHash = ComputeHash(newRawToken);

            // Revoke old one and store new one
            await _tokenRepo.RevokeRefreshTokenAsync(currentToken, ipAddress, newHash);

            var newTokenEntity = new RefreshToken
            {
                TokenHash = newHash,
                UserId = currentToken.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(double.Parse(_config["Jwt:RefreshTokenDays"] ?? "7"))
            };

            await _tokenRepo.AddRefreshTokenAsync(newTokenEntity);
            await _tokenRepo.SaveChangesAsync();
        }

        private static string ComputeHash(string token)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
