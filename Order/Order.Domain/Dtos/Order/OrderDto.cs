using Order.Domain.Dtos.Order;
using Order.Domain.Enums;
using System.Text.Json.Serialization;

namespace Order.Domain.Dtos.Event
{
	public class OrderDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int EventId { get; set; }
		public OrderStatusEnum Status { get; set; }
		public int PaymentMethod { get; set; }
		public long Tickets { get; set; }
		public decimal Price { get; set; }
		public List<OrderItemDto> OrderItemDto { get; set; }
	}
}
