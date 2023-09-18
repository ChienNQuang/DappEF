using DappEF.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DappEF.Implementations;

public abstract class BaseRepository<TEntity> : DbSet<TEntity>, IBaseRepository<TEntity> where TEntity : class
{
    protected BaseRepository(DbContext context)
    {
        DbSet = context.Set<TEntity>();
    }

    private DbSet<TEntity> DbSet { get; }

    #region DbContext methods

    public override IEntityType EntityType
        => DbSet.EntityType;

    public override IAsyncEnumerable<TEntity> AsAsyncEnumerable()
        => DbSet.AsAsyncEnumerable();

    public override IQueryable<TEntity> AsQueryable()
        => DbSet.AsQueryable();

    public override LocalView<TEntity> Local
        => DbSet.Local;
    public override TEntity? Find(params object?[]? keyValues)
        => DbSet.Find(keyValues);

    public override ValueTask<TEntity?> FindAsync(params object?[]? keyValues)
        => DbSet.FindAsync(keyValues);

    public override ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken)
        => DbSet.FindAsync(keyValues, cancellationToken);

    public override EntityEntry<TEntity> Add(TEntity entity)
        => DbSet.Add(entity);

    public override ValueTask<EntityEntry<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
        => DbSet.AddAsync(entity, cancellationToken);

    public override EntityEntry<TEntity> Attach(TEntity entity)
        => DbSet.Attach(entity);

    public override EntityEntry<TEntity> Remove(TEntity entity)
        => DbSet.Remove(entity);

    public override EntityEntry<TEntity> Update(TEntity entity)
        => DbSet.Update(entity);
    
    public override void AddRange(params TEntity[] entities)
        => DbSet.AddRange(entities);
    
    public override void AddRange(IEnumerable<TEntity> entities)
        => DbSet.AddRange(entities);

    public override Task AddRangeAsync(params TEntity[] entities)
        => DbSet.AddRangeAsync(entities);

    public override Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
        => DbSet.AddRangeAsync(entities, cancellationToken);

    public override void AttachRange(params TEntity[] entities)
        => DbSet.AttachRange(entities);

    public override void AttachRange(IEnumerable<TEntity> entities)
        => DbSet.UpdateRange(entities);

    public override void RemoveRange(params TEntity[] entities)
        => DbSet.RemoveRange(entities);

    public override void RemoveRange(IEnumerable<TEntity> entities)
        => DbSet.UpdateRange(entities);

    public override void UpdateRange(params TEntity[] entities)
        => DbSet.UpdateRange(entities);

    public override void UpdateRange(IEnumerable<TEntity> entities)
        => DbSet.UpdateRange(entities);

    public override EntityEntry<TEntity> Entry(TEntity entity)
        => DbSet.Entry(entity);

    public override IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => DbSet.GetAsyncEnumerator(cancellationToken);

    #endregion
}