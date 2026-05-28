using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AxisMart.Application.Ecommerce.User.Customer.Logout;

internal sealed class LogoutCustomerCommandHandler : ICortexCommandHandler<LogoutCustomerCommand, bool>
{
    private readonly IJsonWebTokenRepository _tokenRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCustomerCommandHandler(
        IJsonWebTokenRepository tokenRepository,
        ICustomerRepository customerRepository,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _customerRepository = customerRepository;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(LogoutCustomerCommand request, CancellationToken cancellationToken)
    {
        // 1. Get current authenticated user id from claims
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var customerId))
        {
            return false;
        }

        // 2. Check if customer exists
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        // 3. Get the current JWT token from Authorization header
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var tokenValue = authHeader["Bearer ".Length..].Trim();

        // 4. Find and delete the token from repository (e.g., Redis or DB)
        var existingToken = await _tokenRepository.GetByTokenAsync(tokenValue, cancellationToken);

        if (existingToken is not null)
        {
            existingToken.Revoke("Logout");
            _unitOfWork.Update(existingToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6. Return success
        return true;
    }
}
