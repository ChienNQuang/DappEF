using System.Data;
using DappEF.Contracts;

namespace DappEF.Providers;

public delegate TUnitOfWork UnitOfWorkProvider<out TUnitOfWork>(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    where TUnitOfWork : IUnitOfWork;