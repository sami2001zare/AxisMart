using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;

namespace AxisMart.Application.Ecommerce.User.Customer.ChangePassword;

internal sealed class CustomerChangePasswordCommandHandler(
    ICustomerRepository _customerRepository,
    ICredentialRepository _credentialRepository,
    IUnitOfWork _unitOfWork,
    IPasswordHasher passwordHasher
    ) : ICortexCommandHandler<CustomerChangePasswordCommand, bool>
{
    public async Task<Result<bool>> Handle(CustomerChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByPhoneAsync(request.Phone, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<bool>(new Error("", ""));
        }

        var credential = await _credentialRepository.GetByUserIdAsync(customer.Id, cancellationToken);

        if (credential == null)
        {
            return Result.Failure<bool>(new Error("", ""));
        }

        try
        {
            credential.SetPasswod(passwordHasher.Hash(request.NewPassword));

            _unitOfWork.Update(credential);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            return Result.Failure<bool>(new Error("", ""));
        }
    }
}