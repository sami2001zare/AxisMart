using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using Microsoft.AspNetCore.Http;

namespace AxisMart.Application.Ecommerce.User.Manager.Login;

internal sealed class LoginManagerCommandHandler(
    IAdministratorRepository _customerRepository,
    IJsonWebTokenRepository jwtRepository,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork _unitOfWork,
    IJwtService _jwtService,
    IPasswordHasher passwordHasher) : ICortexCommandHandler<LoginManagerCommand, AccessToken>
{
    public async Task<Result<AccessToken>> Handle(LoginManagerCommand request, CancellationToken cancellationToken)
    {

        Administrator? customer = await _customerRepository.GetGraphAsync(request.Phone, cancellationToken);

        if (customer.Credential!.Hash != passwordHasher.Hash(request.Password))
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
