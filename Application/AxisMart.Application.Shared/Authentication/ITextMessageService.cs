namespace AxisMart.Application.Shared.Authentication;

public interface ITextMessageService
{
    Task SendOTP(string phone, int otp, CancellationToken cancellationToken = default);
}
