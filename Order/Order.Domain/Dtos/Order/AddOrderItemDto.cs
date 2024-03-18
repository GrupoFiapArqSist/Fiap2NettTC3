using System.Text.Json.Serialization;

namespace Order.Domain.Dtos.Order
{
	public class AddOrderItemDto
	{
		[JsonIgnore]
		public int OrderId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }

	}
}
