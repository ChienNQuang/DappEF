using System.Data;
using DappEF.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddDappEF<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
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
    }
}