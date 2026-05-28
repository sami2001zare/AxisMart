using AxisMart.Application.Shared.Authentication;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal sealed class TextMessageService : ITextMessageService
{
    public async Task SendForgotPasswordAsync(string phone, string randomPass, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Password {randomPass} Generated For Customer {phone}");
    }

    public async Task SendOTP(string phone, int otp, CancellationToken cancellationToken = default)
    {
        //Console.Clear();
        Console.WriteLine($"Message {otp} Sent To {phone}");
    }
}