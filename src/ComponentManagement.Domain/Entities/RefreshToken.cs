// ComponentManagement.Domain.Entities/RefreshToken.cs
using System;

namespace ComponentManagement.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TokenHash { get; set; } = null!; // hashed token
        
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByTokenHash { get; set; } // token hash of new token when rotated
        public string? CreatedByIp { get; set; }
        public string? RevokedByIp { get; set; }
        

        public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
    }
}
