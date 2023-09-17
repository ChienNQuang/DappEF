using System.Data;

namespace DappEF.Providers;

public delegate IDbTransaction TransactionProvider(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);