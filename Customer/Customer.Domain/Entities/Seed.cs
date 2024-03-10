using Microsoft.EntityFrameworkCore;

namespace Customer.Domain.Entities;

public abstract class Seed
{
    public Seed(DbContext dbContext)
    {
        DbContext = dbContext;
        EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
    public abstract void Up();
    protected string EnvironmentName { get; private set; }
    protected DbContext DbContext { get; private set; }

    public static Seed CreateInstance(Type seedType, DbContext dbContext, IServiceProvider serviceProvider)
    {
        return Activator.CreateInstance(seedType, dbContext, serviceProvider) as Seed;
    }
}

