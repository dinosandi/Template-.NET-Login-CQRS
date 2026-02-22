// ComponentManagement.Application.Interfaces/ITokenRepository.cs
using ComponentManagement.Domain.Entities;

public interface ITokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash);
    Task RevokeRefreshTokenAsync(RefreshToken token, string revokedByIp, string? replacedByHash = null);
    Task SaveChangesAsync();
}
