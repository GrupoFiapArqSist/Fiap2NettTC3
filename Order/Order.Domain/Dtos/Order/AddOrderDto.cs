namespace Order.Domain.Dtos.Order
{
	public class AddOrderDto
	{
		public int EventId { get; set; }
		public int PaymentMethod { get; set; }
		public List<AddOrderItemDto> AddOrderItemDto { get; set; }
	}
}
