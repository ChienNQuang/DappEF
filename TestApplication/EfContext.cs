using Microsoft.EntityFrameworkCore;

namespace TestApplication;

public class EfContext : DbContext
{
    public EfContext(DbContextOptions<EfContext> options) : base(options)
    {
    }

    public DbSet<Grade> Grades => Set<Grade>();
}
