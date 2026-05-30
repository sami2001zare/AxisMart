using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace AxisMart.Application.Ecommerce.User.Customer.Login;

internal sealed class LoginManagerCommandHandler(
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
        Core.Ecommerce.User.Customer? customer = await _customerRepository.GetByPhoneAsync(new Phone(request.EmailOrPhone), cancellationToken);

        var customerI = await _customerRepository.GetCustomerGraphAsync(customer.Id, cancellationToken);

        string hash = passwordHasher.Hash(request.Password, Convert.FromBase64String(customerI.Credential!.Salt));

        if (customerI.Credential!.Hash != hash)
        {
            return Result.Failure<AccessToken>(new Error("", ""));
        }

        AccessToken token = await _jwtService.GetAccessTokenWithMetadataAsync(customer, cancellationToken);

        JsonWebToken jwt = JsonWebToken.Create(token.Token, token.Expiration, "Login", httpContextAccessor.HttpContext.Request.Headers.UserAgent, "IP Address", customer.Id);

        await jwtRepository.AddAsync(jwt, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }

}
