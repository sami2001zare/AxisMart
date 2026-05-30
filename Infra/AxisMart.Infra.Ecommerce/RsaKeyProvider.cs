using AxisMart.Application.Shared.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace AxisMart.Infra.Ecommerce;

public sealed class RsaKeyProvider : IRsaKeyProvider, IDisposable
{
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;
    private bool _disposed;

    public RsaKeyProvider(IOptions<RsaOptions> options)
    {
        var config = options.Value;

        // ── Private Key ────────────────────────────────────────
        var privateRsa = RSA.Create();
        //privateRsa.ImportFromPem(config.PrivateKeyPem);

        // Validate minimum key size (2048-bit minimum, 4096 recommended)
        if (privateRsa.KeySize < 2048)
            throw new InvalidOperationException(
                $"RSA private key size {privateRsa.KeySize} bits is too small. Minimum is 2048 bits.");

        _privateKey = new RsaSecurityKey(privateRsa)
        {
            KeyId = ComputeKeyId(privateRsa) // kid claim — used for key rotation
        };

        // ── Public Key ─────────────────────────────────────────
        var publicRsa = RSA.Create();
        // publicRsa.ImportFromPem(config.PublicKeyPem);

        _publicKey = new RsaSecurityKey(publicRsa)
        {
            KeyId = ComputeKeyId(publicRsa)  // must match private key's kid
        };
    }

    public RsaSecurityKey GetPrivateKey()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _privateKey;
    }

    public RsaSecurityKey GetPublicKey()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return _publicKey;
    }

    // ── Key ID ─────────────────────────────────────────────────
    // Deterministic kid derived from the public key modulus
    // Allows clients to identify which key to use for validation
    private static string ComputeKeyId(RSA rsa)
    {
        var parameters = rsa.ExportParameters(includePrivateParameters: false);
        var hash = SHA256.HashData(parameters.Modulus!);
        return Convert.ToBase64String(hash)[..16]; // first 16 chars is enough
    }

    public void Dispose()
    {
        if (_disposed) return;

        (_privateKey.Rsa as IDisposable)?.Dispose();
        (_publicKey.Rsa as IDisposable)?.Dispose();

        _disposed = true;
    }
}