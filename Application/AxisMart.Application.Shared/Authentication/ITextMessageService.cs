namespace AxisMart.Application.Shared.Authentication;

public interface ITextMessageService
{
    Task SendOTP(string phone, int otp, CancellationToken cancellationToken = default);
    Task SendForgotPasswordAsync(string phone, string randomPass, CancellationToken cancellationToken = default);
}
