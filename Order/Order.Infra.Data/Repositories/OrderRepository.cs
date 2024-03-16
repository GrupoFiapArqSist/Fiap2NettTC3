using Order.Domain.Interfaces.Repositories;
using Order.Infra.Data.Context;

namespace Order.Infra.Data.Repositories
{
	public class OrderRepository : BaseRepository<Domain.Entities.Order, int, ApplicationDbContext>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext context) : base(context)
		{ }

		public async Task<Domain.Entities.Order> InsertWithReturnId(Domain.Entities.Order obj)
		{
			_dataContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			await _dataContext.SaveChangesAsync();

			return obj;
		}
	}
}
