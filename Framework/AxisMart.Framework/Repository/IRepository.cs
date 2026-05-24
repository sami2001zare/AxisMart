using AxisMart.Framework.Pagination;

namespace AxisMart.Framework.Repository;

public interface IRepository<TEntity, TId>
    where TEntity : Entity
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagedAsync(PagedQuery query, CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagedAsync(Specification<TEntity> specification, PagedQuery query, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}
