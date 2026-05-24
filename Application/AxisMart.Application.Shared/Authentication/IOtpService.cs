namespace AxisMart.Application.Shared.Authentication;

// Application/Interfaces/IOtpService.cs
public interface IOtpService
{
    int Generate(int length = 6);
    bool IsExpired(DateTime expiresAt);
}
