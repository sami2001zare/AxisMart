using AxisMart.Core.Ecommerce.User;
using System.Security.Claims;

namespace AxisMart.Application.Shared.Authentication;

public sealed record AccessToken(string Token, DateTime Expiration);
public interface IJwtService
{
    Task<string> GetAccessTokenAsync(User user, CancellationToken cancellationToken = default);
    Task<AccessToken> GetAccessTokenWithMetadataAsync(User user, CancellationToken cancellationToken = default);
    ClaimsPrincipal? ValidateAccessToken(string token);
    string? GetJtiFromToken(string token);


    string GenerateRefreshToken();
    string HashToken(string rawToken);
}
