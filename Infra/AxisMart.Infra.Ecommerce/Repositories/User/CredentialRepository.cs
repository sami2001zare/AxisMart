using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

//internal sealed class PasswordHasher : IPasswordHasher
//{
//    public string Hash(string password)
//    {
//        return password;
//    }

//    public bool NeedsRehash(string storedHash)
//    {
//        throw new NotImplementedException();
//    }

//    public bool Verify(string password, string storedHash)
//    {
//        throw new NotImplementedException();
//    }
//}


internal sealed class CredentialRepository(AxisMartContext axisMartContext) : ICredentialRepository
{
    public async Task AddAsync(Credential credential, CancellationToken cancellationToken = default)
    {
        await axisMartContext.Credentials.AddAsync(credential, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Credential> credentials, CancellationToken cancellationToken = default)
    {
        await axisMartContext.Credentials.AddRangeAsync(credentials, cancellationToken);
    }

    public Task<IAsyncEnumerable<Credential>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Credential?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
