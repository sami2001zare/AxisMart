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
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace AxisMart.Application.Ecommerce.User.Customer.Register;

public sealed record RegisterCustomerCommand(FirstName FirstName, LastName LastName, Email Email, Phone Phone, string Password) : ICortexCommand<Guid>;

internal sealed class RegisterCustomerCommandHandler(ICustomerRepository _userRepository,
    ICredentialRepository _credentialRepository,
    IPasswordHasher _passwordHasher,
    IUnitOfWork _unitOfWork) : ICortexCommandHandler<RegisterCustomerCommand, Guid>
{


    public async Task<Result<Guid>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        //Validate email uniqueness
        if (await _userRepository.GetByEmailAsync(request.Email, cancellationToken) is not null)
            return Result.Failure<Guid>(new Error("", ""));

        // Create user aggregate
        var user = Core.Ecommerce.User.Customer.Register(Guid.CreateVersion7(), request.FirstName, request.LastName, request.Phone);

        string password = _passwordHasher.Hash(request.Password);

        Credential credential = Credential.Create(password, user.Id);

        await _userRepository.AddAsync(user, cancellationToken);
        await _credentialRepository.AddAsync(credential, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}

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


public sealed record CustomerVaidateRegisterationCommand(Phone Phone, string OTP) : ICortexCommand<string>;

internal sealed class CustomerVaidateRegisterationCommandHandler(
    IOneTimePasswordRepository _otpRepository,
    ICustomerRepository _customerRepository,
    IJsonWebTokenRepository jwtRepository,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork _unitOfWork,
    IJwtService _jwtService
    ) : ICortexCommandHandler<CustomerVaidateRegisterationCommand, string>
{
    public async Task<Result<string>> Handle(CustomerVaidateRegisterationCommand request, CancellationToken cancellationToken)
    {
        OneTimePassword? oneTime = await _otpRepository.GetLatestByPhoneAsync(request.Phone, cancellationToken);

        if (oneTime is null)
        {
            return Result.Failure<string>(new Error("", ""));
        }

        else
        {
            var customer = await _customerRepository.GetByPhoneAsync(request.Phone, cancellationToken);

            if (customer is null)
            {
                return Result.Failure<string>(new Error("", ""));
            }

            else
            {
                customer.VerifyPhone();
                _unitOfWork.Update(customer);

                AccessToken token = await _jwtService.GetAccessTokenWithMetadataAsync(customer, cancellationToken);

                JsonWebToken jwt = JsonWebToken.Create(token.Token, token.Expiration, "Login", httpContextAccessor.HttpContext.Request.Headers.UserAgent, "IP Address", customer.Id);

                await jwtRepository.AddAsync(jwt, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return token.Token;
            }
        }
    }
}


public sealed record AuthResponse(string AccessToken, DateTime ExpiresIn);

public sealed record LoginCustomerCommand(string EmailOrPhone, string Password, bool RememberMe) : ICortexCommand<AccessToken>;

internal sealed class LoginCustomerCommandHandler(
    ICustomerRepository _customerRepository,
    IJsonWebTokenRepository jwtRepository,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork _unitOfWork,
    IJwtService _jwtService,
    IPasswordHasher passwordHasher) : ICortexCommandHandler<LoginCustomerCommand, AccessToken>
{

    public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    //09123456789, +989123456789, 00989123456789, 9123456789
    public const string PersianPhoneRegex = @"^(\+98|0098|98|0)?9\d{9}$";

    public async Task<Result<AccessToken>> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
    {

        string inputType = GetInputType(request.EmailOrPhone);

        if (inputType == "Unknown")
        {
            return Result.Failure<AccessToken>(new Error("", ""));
        }


        Core.Ecommerce.User.Customer? customer = null;

        if (inputType == "Email")
        {
            customer = await _customerRepository.GetByEmailAsync(new Email(request.EmailOrPhone), cancellationToken);

            return Result.Failure<AccessToken>(new Error("", ""));
        }

        else if (inputType == "Phone")
        {
            customer = await _customerRepository.GetByPhoneAsync(new Phone(request.EmailOrPhone), cancellationToken);

            return Result.Failure<AccessToken>(new Error("", ""));
        }

        var customerI = await _customerRepository.GetCustomerGraphAsync(customer.Id, cancellationToken);

        if (customerI.Credential!.Hash != passwordHasher.Hash(request.Password))
        {
            return Result.Failure<AccessToken>(new Error("", ""));

        }


        AccessToken token = await _jwtService.GetAccessTokenWithMetadataAsync(customer, cancellationToken);

        JsonWebToken jwt = JsonWebToken.Create(token.Token, token.Expiration, "Login", httpContextAccessor.HttpContext.Request.Headers.UserAgent, "IP Address", customer.Id);

        await jwtRepository.AddAsync(jwt, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }


    public static string GetInputType(string input)
    {
        if (Regex.IsMatch(input, EmailRegex))
            return "Email";

        if (Regex.IsMatch(input, PersianPhoneRegex))
            return "Phone";

        return "Unknown";
    }
}


public sealed record ChangePasswordByAdminCommand();
public sealed record ChangePasswordByCustomerCommand();
public sealed record ChangePasswordOnLoginCommand();