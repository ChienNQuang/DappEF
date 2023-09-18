using System.Data;
using DappEF.Implementations;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace TestApplication;

public class GradeRepository : BaseRepository<Grade>
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;
    public GradeRepository(DbContext context, IDbConnection connection, IDbTransaction transaction) : base(context)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<IEnumerable<Grade>> GetInRangeAsync(int from, int to)
    {
        var sql = "SELECT id, score " +
                  "FROM grades " +
                  "WHERE @From <= score AND score <= @To";
        var result = await _connection.QueryAsync<Grade>(sql, new
        {
            From = from,
            To = to,
        }, transaction: _transaction);

        return result;
    }
}