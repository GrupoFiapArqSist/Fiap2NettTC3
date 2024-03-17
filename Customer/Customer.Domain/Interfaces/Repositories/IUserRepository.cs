using Customer.Domain.Entities;
using TicketNow.Domain.Interfaces.Repositories;

namespace Customer.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, int>
{
}
