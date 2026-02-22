using ComponentManagement.Domain.Entities;

namespace ComponentManagement.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<(string RawToken, RefreshToken Entity)> CreateNewTokenAsync(User user, string createdByIp = null);
        Task<(string RawToken, RefreshToken Entity)> RotateRefreshTokenAsync(RefreshToken oldToken, string createdByIp, string replacedByIp);
        Task<RefreshToken?> GetValidTokenAsync(string rawToken);
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash);
        Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(Guid userId);
        Task RevokeTokenAsync(RefreshToken token, string revokedByIp, string? reason = null);
    }
}
