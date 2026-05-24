namespace AxisMart.Application.Shared.Authentication;

public interface IMessageService
{
    Task SendVerificationMessageAsync(string phone, string otp, CancellationToken ct = default);
}