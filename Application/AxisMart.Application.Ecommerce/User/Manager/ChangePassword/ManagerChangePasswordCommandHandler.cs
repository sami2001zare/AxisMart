using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;
using System.Security.Cryptography;
using System.Text;

namespace AxisMart.Application.Ecommerce.User.Manager.ChangePassword;

internal sealed class ManagerChangePasswordCommandHandler(
    IAdministratorRepository _customerRepository,
    IUnitOfWork _unitOfWork,
    IPasswordHasher passwordHasher
    ) : ICortexCommandHandler<ManagerChangePasswordCommand, bool>
{
    public async Task<Result<bool>> Handle(ManagerChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetGraphAsync(request.Id, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<bool>(new Error("", ""));
        }

        try
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            customer.Credential!.SetPasswod(passwordHasher.Hash(request.NewPassword, salt));
            customer.Credential!.SetSalt(Convert.ToBase64String(salt));

            _unitOfWork.Update(customer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            return Result.Failure<bool>(new Error("", ""));
        }
    }
}