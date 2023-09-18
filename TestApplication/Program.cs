using System.Data;
using DappEF.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EfContext>(opt =>
{
    opt.UseNpgsql("Host=localhost;Port=5432;User ID=postgres;Password=postgres;Database=postgres").UseCamelCaseNamingConvention();
});
builder.Services.AddDappEF<EfContext>();
builder.Services.AddUnitOfWork<ApplicationDbContext>();
builder.Services.AddScoped<GradeRepository>();

var app = builder.Build();

app.MapGet("/", async ([FromServices] UnitOfWorkProvider<ApplicationDbContext> contextProvider) =>
{
    var context = contextProvider.Invoke(IsolationLevel.Snapshot);
    var grades = await context.Grades.GetInRangeAsync(0, 5);
    return grades.Count();
});

app.Run();
