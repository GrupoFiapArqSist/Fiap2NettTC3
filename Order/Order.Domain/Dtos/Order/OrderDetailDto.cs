using Order.Domain.Enums;
using TicketNow.Domain.Extensions;

namespace Order.Domain.Dtos.Order
{
	public class OrderDetailsDto
	{
		public int Id { get; set; }
		public int EventId { get; set; }
		public string Status { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentStatus { get; set; }
		public long Tickets { get; set; }
		public decimal PriceAmount { get; set; }
		public List<OrderItemDto> OrderItemDto { get; set; }

		public OrderDetailsDto(int id, int eventId, OrderStatusEnum statusEnum, PaymentStatusEnum pStatusEnum, int methodEnum, long tickets,
			decimal priceAmount, List<OrderItemDto> ltOrderItemDto)
		{
			Id = id;
			EventId = eventId;
			Status = statusEnum.GetEnumDescription();
			PaymentMethod = ReturnPaymentMethodEnum(methodEnum).GetEnumDescription();
			PaymentStatus = pStatusEnum.GetEnumDescription();
			Tickets = tickets;
			PriceAmount = priceAmount;
			OrderItemDto = ltOrderItemDto;
		}

		public static PaymentMethodEnum ReturnPaymentMethodEnum(int idMethod)
		{
			switch (idMethod)
			{
				case 1:
					return PaymentMethodEnum.Ticket;
				case 2:
					return PaymentMethodEnum.Pix;
				default:
					return PaymentMethodEnum.CreditCard;
			}
		}
	}
}
