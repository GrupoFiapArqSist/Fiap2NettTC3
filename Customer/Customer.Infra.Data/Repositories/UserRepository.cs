using Customer.Domain.Entities;
using Customer.Domain.Interfaces.Repositories;
using Customer.Infra.Data.Context;
using TicketNow.Infra.Data.Repositories;

namespace Customer.Infra.Data.Repositories;

public class UserRepository : BaseRepository<User, int, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
