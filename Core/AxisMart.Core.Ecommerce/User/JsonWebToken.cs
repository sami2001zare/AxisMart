using AxisMart.Framework;

namespace AxisMart.Core.Ecommerce.User;

public sealed partial class JsonWebToken : Entity
{
    public JsonWebToken(Guid id, string token, DateTime expiration, string usedFor, string userAgent, string ip, Guid userId)
    {
        Id = id;
        Token = token;
        Expiration = expiration;
        UsedFor = usedFor;
        UserAgent = userAgent;
        IpAddress = ip;
        UserId = userId;
    }

    public string Token { get; private set; } = string.Empty!;
    public bool RememberMe { get; private set; }
    public DateTime Expiration { get; set; }

    public bool IsRevoked { get; private set; } = false;
    public DateTimeOffset? RevokedAt { get; private set; }
    public string? RevokedReason { get; private set; }

    public string UsedFor { get; set; } = string.Empty!; // [RefreshToken / Login]
    public string IpAddress { get; private set; } = string.Empty!;
    public string UserAgent { get; set; } = string.Empty!;

    public DateTimeOffset CreatedAt { get; private set; }


    public Guid UserId { get; set; }
    public User? User { get; set; }


    // Domain methods
    public bool IsExpired() => DateTimeOffset.UtcNow >= Expiration;

    public bool IsActive() => !IsRevoked && !IsExpired();

    public void Revoke(string reason)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Token is already revoked");

        IsRevoked = true;
        RevokedAt = DateTimeOffset.UtcNow;
        RevokedReason = reason;
    }

    public static JsonWebToken Create(string token, DateTime expiration, string usedFor, string userAgent, string ip, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty", nameof(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("TokenHash cannot be null or empty", nameof(token));

        if (expiration <= DateTimeOffset.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future", nameof(expiration));

        if (string.IsNullOrWhiteSpace(ip))
            throw new ArgumentException("IpAddress cannot be null or empty", nameof(ip));

        if (string.IsNullOrWhiteSpace(userAgent))
            throw new ArgumentException("UserAgent cannot be null or empty", nameof(userAgent));


        JsonWebToken jsonWebToken = new(Guid.CreateVersion7(), token, expiration, usedFor, userAgent, ip, userId);

        return jsonWebToken;
    }
}
