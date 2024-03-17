namespace Order.Domain.Interfaces.Repositories;
public interface IOrderRepository : IBaseRepository<Domain.Entities.Order, int>
{
	Task<Domain.Entities.Order> InsertWithReturnId(Domain.Entities.Order obj);
}
