using Order.Domain.Interfaces.Entities;

namespace Order.Domain.Entities
{
	public class OrderItem : BaseEntity, IEntity<int>
	{
		public int OrderId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public virtual Order Order { get; set; }

		public OrderItem()
		{

		}

		public OrderItem(int orderId, string name, string email)
		{
			CreatedAt = DateTime.Now;
			OrderId = orderId;
			Name = name;
			Email = email;
		}
	}
}
