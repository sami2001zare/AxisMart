using AxisMart.Core.Ecommerce.User;
using AxisMart.Core.Ecommerce.User.Repositpry;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AxisMart.Infra.Ecommerce.Repositories.User;

internal class CustomerRepository(AxisMartContext axisMartContext) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddAsync(customer, cancellationToken);
    }

    public async Task AddRangeAsync(ReadOnlyMemory<Customer> customers, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddRangeAsync(customers, cancellationToken);
    }

    public async Task<IAsyncEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return axisMartContext.Customers.ToAsyncEnumerable();
    }

    public async Task<Customer?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByPhoneAsync(Phone phone, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Customers.FirstOrDefaultAsync(c => c.Phone == phone, cancellationToken);
    }

    public async Task<Customer> GetCustomerGraphAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Customers.Where(c => c.Id == customerId)
            .Include(c => c.Credential)
            .FirstAsync(cancellationToken);
    }
}


internal class AdministratorRepository(AxisMartContext axisMartContext) : IAdministratorRepository
{
    public async Task AddAsync(Administrator administrator, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddAsync(administrator, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Administrator> administrators, CancellationToken cancellationToken = default)
    {
        await axisMartContext.AddRangeAsync(administrators, cancellationToken);
    }

    public async Task<IAsyncEnumerable<Administrator>> GetAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return axisMartContext.Administrators.AsAsyncEnumerable();
    }

    public async Task<Administrator?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Administrators.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<Administrator> GetGraphAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Administrators.Where(c => c.Id == customerId)
            .Include(c => c.Credential)
            .FirstAsync(cancellationToken);
    }

    public async Task<Administrator> GetGraphAsync(Phone phone, CancellationToken cancellationToken = default)
    {
        return await axisMartContext.Administrators.Where(c => c.Phone == phone)
            .Include(c => c.Credential)
            .FirstAsync(cancellationToken);
    }
}