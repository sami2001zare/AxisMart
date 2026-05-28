using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Clock;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Events;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using MediatR;

namespace AxisMart.Application.Ecommerce.User.Customer.Register;

public sealed record RegisterCustomerCommand(FirstName FirstName, LastName LastName, Email Email, Phone Phone, string Password) : ICortexCommand<Guid>;


// This Events Creates An 6 Digit OTP Code With Expiration And Expiration Time Skew
internal sealed class CustomerFirstStepRegisteredDomainEventHandler(
    ICustomerRepository _userRepository,
    IOtpService _otpService,
    IDateTimeProvider _dateTimeProvider,
    IOneTimePasswordRepository _otpRepository,
    IUnitOfWork _unitOfWork) : INotificationHandler<CustomerFirstStepRegisteredEvent>
{

    public async Task Handle(CustomerFirstStepRegisteredEvent notification, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByPhoneAsync(notification.Phone, cancellationToken) is null)
        {
            Result.Failure<Guid>(new Error("", ""));
        }

        else
        {
            DateTime dateTime = _dateTimeProvider.UtcNow;

            OneTimePassword otp = OneTimePassword.Create(Guid.CreateVersion7(),
                _otpService.Generate(),
                notification.Phone.Value,
                dateTime,
                dateTime.AddMinutes(5));

            await _otpRepository.AddAsync(otp, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

//OneTimePasswordIssuedDomainEvent - Send SMS Or Email Notification
internal sealed class OneTimePasswordIssuedDomainEventHandler(IOneTimePasswordRepository _otpRepository, ITextMessageService _textMessageService) : INotificationHandler<OneTimePasswordIssuedDomainEvent>
{
    public async Task Handle(OneTimePasswordIssuedDomainEvent notification, CancellationToken cancellationToken)
    {
        OneTimePassword? oneTime = await _otpRepository.GetLatestByPhoneAsync(notification.Phone, cancellationToken);

        if (oneTime is null)
        {
            Result.Failure<Guid>(new Error("", ""));
        }

        else
        {
            try
            {
                await _textMessageService.SendOTP(oneTime.EmailOrPhone, oneTime.Code, cancellationToken);
                Result.Success(Guid.CreateVersion7());
            }
            catch (Exception)
            {
                Result.Failure<Guid>(new Error("", ""));
            }
        }
    }
}


public sealed record AuthResponse(string AccessToken, DateTime ExpiresIn);

public sealed record ChangePasswordByAdminCommand();
public sealed record ChangePasswordByCustomerCommand();
public sealed record ChangePasswordOnLoginCommand();