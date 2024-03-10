using Customer.Domain.Entities;
using Customer.Domain.Interfaces.Repositories;
using Customer.Infra.Data.Context;

namespace Customer.Infra.Data.Repositories;

public class UserRepository : BaseRepository<User, int, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
