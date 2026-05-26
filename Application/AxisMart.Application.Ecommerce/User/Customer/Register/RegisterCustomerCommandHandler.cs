using AxisMart.Application.Shared.Authentication;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Framework;
using AxisMart.Framework.Repository;

namespace AxisMart.Application.Ecommerce.User.Customer.Register;

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
