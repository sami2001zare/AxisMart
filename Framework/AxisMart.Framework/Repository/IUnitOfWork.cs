using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AxisMart.Framework.Repository;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry Update(object entity);
    EntityEntry Remove(object entity);
}
