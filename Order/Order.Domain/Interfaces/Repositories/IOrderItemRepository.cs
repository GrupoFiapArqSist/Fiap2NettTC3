using Order.Domain.Entities;

namespace Order.Domain.Interfaces.Repositories
{
	public interface IOrderItemRepository : IBaseRepository<OrderItem, int>
	{
	}
}
