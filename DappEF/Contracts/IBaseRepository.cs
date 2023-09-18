using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DappEF.Contracts;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public IEntityType EntityType { get; }

    public IAsyncEnumerable<TEntity> AsAsyncEnumerable();

    public IQueryable<TEntity> AsQueryable();

    public LocalView<TEntity> Local { get; }
    public TEntity? Find(params object?[]? keyValues);

    public ValueTask<TEntity?> FindAsync(params object?[]? keyValues);

    public ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken);

    public EntityEntry<TEntity> Add(TEntity entity);

    public ValueTask<EntityEntry<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    public EntityEntry<TEntity> Attach(TEntity entity);

    public EntityEntry<TEntity> Remove(TEntity entity);

    public EntityEntry<TEntity> Update(TEntity entity);

    public void AddRange(params TEntity[] entities);

    public void AddRange(IEnumerable<TEntity> entities);

    public Task AddRangeAsync(params TEntity[] entities);

    public Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    public void AttachRange(params TEntity[] entities);

    public void AttachRange(IEnumerable<TEntity> entities);

    public void RemoveRange(params TEntity[] entities);

    public void RemoveRange(IEnumerable<TEntity> entities);

    public void UpdateRange(params TEntity[] entities);

    public void UpdateRange(IEnumerable<TEntity> entities);

    public EntityEntry<TEntity> Entry(TEntity entity);

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default);

}