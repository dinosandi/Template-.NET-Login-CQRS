// ComponentManagement.Infrastructure/Repositories/TokenRepository.cs
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;
    public TokenRepository(AppDbContext context) => _context = context;

    public async Task AddRefreshTokenAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
    }

    public async Task RevokeRefreshTokenAsync(RefreshToken token, string revokedByIp, string? replacedByHash = null)
    {
        token.RevokedAt = DateTime.UtcNow;
        token.RevokedByIp = revokedByIp;
        token.ReplacedByTokenHash = replacedByHash;
        _context.RefreshTokens.Update(token);
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
