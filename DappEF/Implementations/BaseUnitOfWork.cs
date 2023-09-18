using System.Data;
using DappEF.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DappEF.Implementations;

public abstract class BaseUnitOfWork : IUnitOfWork
{
    protected readonly IDbTransaction Transaction;
    protected readonly DbContext Context;

    protected BaseUnitOfWork(IDbTransaction transaction, DbContext context)
    {
        Transaction = transaction;
        Context = context;
    }

    public void Dispose()
    {
        Transaction.Connection?.Close();
        Transaction.Connection?.Dispose();
        Transaction.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task CommitAsync()
    {
        try
        {
            await Context.SaveChangesAsync();
            Transaction.Commit();
        }
        catch
        {
            Transaction.Rollback();
        }
    }
}