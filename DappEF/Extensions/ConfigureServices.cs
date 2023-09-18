using System.Data;
using System.Reflection;
using DappEF.Contracts;
using DappEF.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddDappEF<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<DbContext>(s => s.GetRequiredService<TDbContext>());

        services.AddScoped<IDbConnection>(s =>
        {
            var dbContext = s.GetRequiredService<DbContext>();
            var connection = dbContext.Database.GetDbConnection();
            return connection;
        });
        
        services.AddScoped<TransactionProvider>(s => i =>
        {
            var dbContext = s.GetRequiredService<DbContext>();
            var dbContextTransaction = dbContext.Database.CurrentTransaction ?? dbContext.Database.BeginTransaction(i);
            return dbContextTransaction.GetDbTransaction();
        });
        
        services.AddScoped<IDbTransaction>(s =>
        {
            var transactionProvider = s.GetRequiredService<TransactionProvider>();
            return transactionProvider.Invoke();
        });
    }

    public static void AddUnitOfWork<TUnitOfWorkService, TUnitOfWorkImplementation>(this IServiceCollection services)
        where TUnitOfWorkService : IUnitOfWork
        where TUnitOfWorkImplementation : class, TUnitOfWorkService
    {
        services.AddScoped<UnitOfWorkProvider<TUnitOfWorkService>>(s => i =>
        {
            var transactionProvider = s.GetRequiredService<TransactionProvider>();
            var dbTransaction = transactionProvider.Invoke(i);
            return (TUnitOfWorkImplementation)ResolveService(s, typeof(TUnitOfWorkImplementation), dbTransaction);
        });

        services.AddScoped(typeof(TUnitOfWorkService), typeof(TUnitOfWorkImplementation));
    }
    
    public static void AddUnitOfWork<TUnitOfWork>(this IServiceCollection services)
        where TUnitOfWork : class, IUnitOfWork
    {
        services.AddScoped<UnitOfWorkProvider<TUnitOfWork>>(s => i =>
        {
            var transactionProvider = s.GetRequiredService<TransactionProvider>();
            var dbTransaction = transactionProvider.Invoke(i);
            return (TUnitOfWork)ResolveService(s, typeof(TUnitOfWork), dbTransaction);
        });

        services.AddScoped(typeof(TUnitOfWork));
    }

    private static object ResolveService(IServiceProvider provider, Type serviceType, IDbTransaction dbTransaction)
    {
        if (!serviceType.GetConstructors().Any())
        {
            return provider.GetRequiredService(serviceType);
        }
        var constructor = serviceType.GetConstructors().First();
        var parameters = constructor.GetParameters();
        if (parameters.All(x => x.ParameterType != typeof(IDbTransaction)))
        {
            return provider.GetRequiredService(serviceType);
        }

        var arguments = new object[parameters.Length];
        for (var index = 0; index < parameters.Length; index++)
        {
            if (parameters[index].ParameterType == typeof(IDbTransaction))
            {
                arguments[index] = dbTransaction;
            }
            else
            {
                arguments[index] = ResolveService(provider, parameters[index].ParameterType, dbTransaction);
            }
        }
        return Activator.CreateInstance(serviceType, BindingFlags.Default, null, arguments, null)!;
    }
}