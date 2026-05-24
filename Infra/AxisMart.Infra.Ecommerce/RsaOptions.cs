namespace AxisMart.Infra.Ecommerce;

public sealed class RsaOptions
{
    public const string Section = "Rsa";

    public string PrivateKeyPem { get; init; } = default!;
    public string PublicKeyPem { get; init; } = default!;
}
