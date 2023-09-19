# DappEF

DappEF helps you combine EF Core features with Dapper's speed through predefined base classes.

## Usages:

### Enable Dapper and EF Core operation to work together:

Register with `IServiceProvider`:

```csharp
builder.Services.AddDappEF<SampleContext>();
```

Create a unit of work and depend on it is class or interface, inherit/implement it from `BaseUnitOfWork`/`IUnitOfWork`:

```csharp
public class UnitOfWork : BaseUnitOfWork
{
    public UnitOfWork(IDbTransaction transaction, DbContext context) : base(transaction, context)
    {
    }
}
```

This is the minimal setup.

To use it, with each entity, for example, `Driver` that is already declared in `SampleContext`, create a repository for it, instead of using the `DbSet<>` class directly, and let it inherits from `BaseRepository`:

```csharp
public class DriverRepository : BaseRepository<Driver>
{
    public DriverRepository(DbContext context) : base(context)
    {
    }
}
```

Declare and inject it in the UnitOfWork class:

```csharp
public class UnitOfWork : BaseUnitOfWork
{
    public UnitOfWork(IDbTransaction transaction, DbContext context,
        DriverRepository drivers) : base(transaction, context)
    {
        Drivers = drivers;
    }

    public DriverRepository Drivers { get; }
}
```

This `Drivers` instance is automatically an instance of `DbSet<>`, and we can extend it with Dapper like this:

Inject an `IDbConnection` and an `IDbTransaction`:

```csharp
public class DriverRepository : BaseRepository<Driver>
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;
    public DriverRepository(DbContext context, IDbConnection connection, IDbTransaction transaction) : base(context)
    {
        _connection = connection;
        _transaction = transaction;
    }
}
```

and write methods on those interfaces:

```csharp
public async Task<Driver?> CreateAsync(Driver entity)
{
    var sql = "INSERT INTO Drivers(Id, Name) " +
              "VALUES (@Id, @Name)";
    var result = await _connection.ExecuteScalarAsync<Driver>(sql, entity, _transaction);
    return result;
}
```

and this will automatically work with EF Core, the thing that work is they all share the same transaction, giving us developers the flexiblity over rich features in EF with the speed of Dapper.