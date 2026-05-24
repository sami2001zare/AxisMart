using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Application.Shared.Authentication;

public interface IEmailService
{
    Task SendVerificationEmailAsync(Email to, string token, CancellationToken ct = default);
    Task SendPasswordResetEmailAsync(Email to, string token, CancellationToken ct = default);
    Task SendOtpEmailAsync(Email to, string otp, CancellationToken ct = default);
    Task SendSecurityAlertAsync(Email to, string message, CancellationToken ct = default);
}
