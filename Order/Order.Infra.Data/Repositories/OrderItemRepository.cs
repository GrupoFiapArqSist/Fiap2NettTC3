using Order.Domain.Entities;
using Order.Domain.Interfaces.Repositories;
using Order.Infra.Data.Context;
using TicketNow.Infra.Data.Repositories;

namespace Order.Infra.Data.Repositories;

public class OrderItemRepository : BaseRepository<OrderItem, int, ApplicationDbContext>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
