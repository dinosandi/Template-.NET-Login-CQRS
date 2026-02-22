using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ComponentManagement.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;

        public RefreshTokenService(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================================
        // 🔹 Helper: Generate token acak (64 byte)
        // ==========================================================
        private static string GenerateSecureToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        // ==========================================================
        // 🔹 Helper: Hash token agar tidak tersimpan plaintext di DB
        // ==========================================================
        private static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        // ==========================================================
        // 🔹 Membuat refresh token baru
        // ==========================================================
        public async Task<(string RawToken, RefreshToken Entity)> CreateNewTokenAsync(User user, string createdByIp = null)
        {
            var rawToken = GenerateSecureToken();
            var tokenHash = HashToken(rawToken);

            var entity = new RefreshToken
            {
                TokenHash = tokenHash,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = createdByIp ?? "system"
            };

            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();

            return (rawToken, entity);
        }

        // ==========================================================
        // 🔹 Mengambil token valid berdasarkan plaintext (raw token)
        // ==========================================================
        public async Task<RefreshToken?> GetValidTokenAsync(string rawToken)
        {
            var hash = HashToken(rawToken);

            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>
                    r.TokenHash == hash &&
                    r.RevokedAt == null &&
                    r.ExpiresAt > DateTime.UtcNow);
        }

        // ==========================================================
        // 🔹 Mengambil token berdasarkan hash
        // ==========================================================
        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);
        }

        // ==========================================================
        // 🔹 Mendapatkan semua refresh token milik user
        // ==========================================================
        public async Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // ==========================================================
        // 🔹 Rotasi token (buat token baru & revoke yang lama)
        // ==========================================================
        public async Task<(string RawToken, RefreshToken Entity)> RotateRefreshTokenAsync(
            RefreshToken oldToken,
            string createdByIp,
            string replacedByIp)
        {
            // Buat token baru
            var (rawToken, newEntity) = await CreateNewTokenAsync(oldToken.User, createdByIp);

            // Revoke token lama
            oldToken.RevokedAt = DateTime.UtcNow;
            oldToken.RevokedByIp = replacedByIp;
            oldToken.ReplacedByTokenHash = newEntity.TokenHash;

            _context.RefreshTokens.Update(oldToken);
            await _context.SaveChangesAsync();

            return (rawToken, newEntity);
        }

        // ==========================================================
        // 🔹 Revoke token secara manual
        // ==========================================================
        public async Task RevokeTokenAsync(RefreshToken token, string revokedByIp, string? reason = null)
        {
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = revokedByIp;

            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
