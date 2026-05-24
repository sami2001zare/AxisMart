namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface IAdministratorRepository
{
    Task<Administrator> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IAsyncEnumerable<Administrator>> GetAllAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Administrator administrator, CancellationToken cancellationToken = default);
    Task AddRangeAsync(ReadOnlySpan<Administrator> administrators, CancellationToken cancellationToken = default);
}
