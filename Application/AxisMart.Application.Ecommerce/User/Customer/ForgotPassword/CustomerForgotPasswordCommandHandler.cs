using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Generator;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;

namespace AxisMart.Application.Ecommerce.User.Customer.ForgotPassword;

internal sealed class ManagerForgotPasswordCommandHandler(
    ICustomerRepository _customerRepository,
    ICredentialRepository _credentialRepository,
    IUnitOfWork _unitOfWork,
    IIdGenerator idGenerator,
    IPasswordHasher passwordHasher,
    ITextMessageService textMessageService
    ) : ICortexCommandHandler<CustomerForgotPasswordCommand, string>
{
    public async Task<Result<string>> Handle(CustomerForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByPhoneAsync(request.Phone, cancellationToken);

        if (customer == null)
        {
            return Result.Failure<string>(new Error("", ""));
        }

        var credential = await _credentialRepository.GetByUserIdAsync(customer.Id, cancellationToken);

        if (credential == null)
        {
            return Result.Failure<string>(new Error("", ""));
        }

        try
        {
            string randomPass = await idGenerator.GenerateRandomPassword();
            credential.SetPasswod(passwordHasher.Hash(randomPass));

            _unitOfWork.Update(credential);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await textMessageService.SendForgotPasswordAsync(request.Phone.Value, randomPass, cancellationToken);

            return Result.Success(randomPass);
        }
        catch (Exception)
        {
            return Result.Failure<string>(new Error("", ""));
        }
    }
}
