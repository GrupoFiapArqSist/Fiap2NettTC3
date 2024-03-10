using Customer.Domain.Entities;

namespace Customer.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User, int>
{
}
