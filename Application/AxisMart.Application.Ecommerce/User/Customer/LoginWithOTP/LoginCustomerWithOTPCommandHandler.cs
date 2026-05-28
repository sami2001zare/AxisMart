using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Clock;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using System.Text.RegularExpressions;

namespace AxisMart.Application.Ecommerce.User.Customer.LoginWithOTP;

internal sealed class LoginCustomerWithOTPCommandHandler(
    ICustomerRepository _customerRepository,
    IOtpService _otpService,
    IDateTimeProvider _dateTimeProvider,
    IOneTimePasswordRepository _otpRepository,
    IUnitOfWork _unitOfWork) : ICortexCommandHandler<LoginCustomerWithOTPCommand>
{

    public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    //09123456789, +989123456789, 00989123456789, 9123456789
    public const string PersianPhoneRegex = @"^(\+98|0098|98|0)?9\d{9}$";

    public async Task<Result> Handle(LoginCustomerWithOTPCommand request, CancellationToken cancellationToken)
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

        DateTime dateTime = _dateTimeProvider.UtcNow;

        OneTimePassword otp = OneTimePassword.Create(Guid.CreateVersion7(),
            _otpService.Generate(),
            request.EmailOrPhone,
            dateTime,
            dateTime.AddMinutes(5));

        await _otpRepository.AddAsync(otp, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
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
