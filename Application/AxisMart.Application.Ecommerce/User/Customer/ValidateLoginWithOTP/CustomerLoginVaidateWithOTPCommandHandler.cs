using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using Microsoft.AspNetCore.Http;

namespace AxisMart.Application.Ecommerce.User.Customer.ValidateLoginWithOTP;

internal sealed class CustomerLoginVaidateWithOTPCommandHandler(
    IOneTimePasswordRepository _otpRepository,
    ICustomerRepository _customerRepository,
    IJsonWebTokenRepository jwtRepository,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork _unitOfWork,
    IJwtService _jwtService
    ) : ICortexCommandHandler<CustomerLoginVaidateWithOTPCommand, string>
{
    public async Task<Result<string>> Handle(CustomerLoginVaidateWithOTPCommand request, CancellationToken cancellationToken)
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
