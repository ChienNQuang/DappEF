using System.Data;
using DappEF;
using DappEF.Implementations;
using Microsoft.EntityFrameworkCore;

namespace TestApplication;

public class ApplicationDbContext : BaseUnitOfWork
{
    public ApplicationDbContext(IDbTransaction transaction, DbContext context,
        GradeRepository grades) : base(transaction, context)
    {
        Grades = grades;
    }
 
    public GradeRepository Grades { get; }
}