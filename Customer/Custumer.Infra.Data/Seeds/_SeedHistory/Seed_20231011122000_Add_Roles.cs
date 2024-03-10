using Customer.Domain.Entities;
using Customer.Domain.Utilities;
using Customer.Infra.Data.Context;

namespace Customer.Infra.Data.Seeds._SeedHistory;

public class Seed_20231011122000_Add_Roles : Seed
{
    private readonly ApplicationDbContext _dbContext;
    public Seed_20231011122000_Add_Roles(ApplicationDbContext dbContext,
        IServiceProvider serviceProvider) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Up()
    {
        var roles = new List<Role>
            {
                new Role(StaticUserRoles.PROMOTER),
                new Role(StaticUserRoles.ADMIN),
                new Role(StaticUserRoles.CUSTOMER)
            };

        _dbContext.Roles.AddRange(roles);
        _dbContext.SaveChanges();
    }
}
