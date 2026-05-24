using System.Security.Cryptography;

namespace AxisMart.Application.Shared.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    // Current recommended parameters
    private const int CurrentIterations = 600_000; // OWASP 2023 recommendation for PBKDF2-SHA512
    private const int SaltSize = 16;      // 128-bit salt
    private const int HashSize = 32;      // 256-bit output
    private HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    private const char Delimiter = '$';

    // Format: pbkdf2$SHA512$600000$<base64salt>$<base64hash>

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password: password,
            salt: salt,
            iterations: CurrentIterations,
            hashAlgorithm: Algorithm,
            outputLength: HashSize
        );

        return string.Join(Delimiter,
            "pbkdf2",
            Algorithm.Name,
            CurrentIterations,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash)
        );
    }

    public bool Verify(string password, string storedHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedHash);

        try
        {
            var parts = storedHash.Split(Delimiter);

            if (parts.Length != 5 || parts[0] != "pbkdf2")
                return false;

            var algorithm = new HashAlgorithmName(parts[1]);
            var iterations = int.Parse(parts[2]);
            var salt = Convert.FromBase64String(parts[3]);
            var expected = Convert.FromBase64String(parts[4]);

            var actual = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: iterations,
                hashAlgorithm: algorithm,
                outputLength: expected.Length
            );

            // Constant-time comparison — prevents timing attacks
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch
        {
            return false;
        }
    }

    public bool NeedsRehash(string storedHash)
    {
        try
        {
            var parts = storedHash.Split(Delimiter);

            if (parts.Length != 5 || parts[0] != "pbkdf2")
                return true;

            var algorithm = new HashAlgorithmName(parts[1]);
            var iterations = int.Parse(parts[2]);

            // Rehash if algorithm or iterations are weaker than current
            return algorithm != Algorithm ||
                   iterations < CurrentIterations;
        }
        catch
        {
            return true; // If parsing fails, force rehash
        }
    }
}
