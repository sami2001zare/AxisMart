using AxisMart.Application.Shared.Authentication;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

public sealed class OtpService : IOtpService
{
    public int Generate(int length = 6)
    {
        return Random.Shared.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length) - 1);
    }

    public bool IsExpired(DateTime expiresAt)
    {
        return DateTime.UtcNow > expiresAt;
    }
}
