using AxisMart.Core.Ecommerce.User.ValueObjects;

namespace AxisMart.Core.Ecommerce.User.Repositpry;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Customer?> GetByPhoneAsync(Phone phone, CancellationToken cancellationToken = default);
    Task<Customer> GetCustomerGraphAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IAsyncEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task AddRangeAsync(ReadOnlyMemory<Customer> customers, CancellationToken cancellationToken = default);
}
