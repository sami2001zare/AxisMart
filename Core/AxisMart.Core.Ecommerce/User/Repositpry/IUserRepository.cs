namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IAsyncEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task AddRangeAsync(ReadOnlySpan<User> users, CancellationToken cancellationToken = default);
}
