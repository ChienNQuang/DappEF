namespace TestApplication;

public interface IApplicationDbContext : DappEF.Contracts.IUnitOfWork
{
    GradeRepository Grades { get; }
}