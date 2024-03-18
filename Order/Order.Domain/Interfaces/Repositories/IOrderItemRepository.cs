using Order.Domain.Entities;
using TicketNow.Domain.Interfaces.Repositories;

namespace Order.Domain.Interfaces.Repositories;

public interface IOrderItemRepository : IBaseRepository<OrderItem, int>
{
}
