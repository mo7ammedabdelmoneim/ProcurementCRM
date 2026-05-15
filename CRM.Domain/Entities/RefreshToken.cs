using CRM.Domain.Common;

namespace CRM.Domain.Entities;
<<<<<<< HEAD
=======

namespace CRM.Domain.Entities;

>>>>>>> b80a905c9c1d6b724a8dcb8ee2dd9f97a92ce30d
public class RefreshToken : BaseEntity
{
    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public RefreshToken(string token, Guid userId, int expiryDays)
    {
        Token = token;
        UserId = userId;
        ExpiresAt = DateTime.UtcNow.AddDays(expiryDays);
    }

    public void Revoke(string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}