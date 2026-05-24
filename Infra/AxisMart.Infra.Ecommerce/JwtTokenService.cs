using AxisMart.Application.Shared.Authentication;
using AxisMart.Core.Ecommerce.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AxisMart.Infra.Ecommerce;

// Infrastructure/Services/JwtTokenService.cs
public sealed class JwtTokenService : IJwtService
{
    private readonly JwtOptions _jwtOptions;
    private readonly IRsaKeyProvider _keyProvider;

    public JwtTokenService(
        IOptions<JwtOptions> jwtOptions,
        IRsaKeyProvider keyProvider)
    {
        _jwtOptions = jwtOptions.Value;
        _keyProvider = keyProvider;
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetAccessTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var privateKey = _keyProvider.GetPrivateKey();
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,  user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.Role, user is Customer ? "customer" : user is Administrator ? "manager" : ""),
            new("email_verified", false.ToString().ToLower()),
        };

        //claims.AddRange(
        //    user.Permissions.Select(p => new Claim("permission", p.ToString()))
        //);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public Task<AccessToken> GetAccessTokenWithMetadataAsync(User user, CancellationToken cancellationToken = default)
    {
        var privateKey = _keyProvider.GetPrivateKey();
        var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.Role, user is Customer ? "customer" : user is Administrator ? "manager" : ""),
            new("email_verified", false.ToString().ToLower()),
        };

        //claims.AddRange(
        //    user.Permissions.Select(p => new Claim("permission", p.ToString()))
        //);

        var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
            signingCredentials: credentials
        );

        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(new AccessToken(tokenResult, expiration));
    }

    public string? GetJtiFromToken(string token)
    {
        throw new NotImplementedException();
    }

    public string HashToken(string rawToken)
    {
        throw new NotImplementedException();
    }

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        throw new NotImplementedException();
    }
}
