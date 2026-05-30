namespace AxisMart.Application.Shared.Authentication;

public interface IPasswordHasher
{
    string Hash(string password, byte[] salt);
    bool Verify(string password, string storedHash);
    bool NeedsRehash(string storedHash);
}

//public interface IDateTimeProvider
//{
//    DateTime UtcNow { get; }
//}