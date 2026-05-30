using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface IAdministratorRepository
{
    Task<Administrator?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IAsyncEnumerable<Administrator>> GetAllAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Administrator> GetGraphAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<Administrator> GetGraphAsync(Phone phone, CancellationToken cancellationToken = default);
    Task AddAsync(Administrator administrator, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Administrator> administrators, CancellationToken cancellationToken = default);
}
