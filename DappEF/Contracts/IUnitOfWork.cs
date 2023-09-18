namespace DappEF.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
}