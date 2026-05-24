namespace AxisMart.Infra.Ecommerce;

public sealed class JwtOptions
{
    public const string Section = "Jwt";

    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string PrivateKeyPem { get; init; } = default!;
    public string PublicKeyPem { get; init; } = default!;
    public int AccessTokenExpiryMinutes { get; init; } = 15;
    public int RefreshTokenExpiryDays { get; init; } = 7;
}
