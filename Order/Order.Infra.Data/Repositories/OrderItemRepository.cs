using Order.Domain.Entities;
using Order.Domain.Interfaces.Repositories;
using Order.Infra.Data.Context;

namespace Order.Infra.Data.Repositories
{
	public class OrderItemRepository : BaseRepository<OrderItem, int, ApplicationDbContext>, IOrderItemRepository
	{
		public OrderItemRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
