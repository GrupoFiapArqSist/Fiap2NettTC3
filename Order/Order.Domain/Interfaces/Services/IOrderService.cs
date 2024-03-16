using Order.Domain.Dtos.Default;
using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.MockPayment;
using Order.Domain.Dtos.Order;
using Order.Domain.Filters;

namespace Order.Domain.Interfaces.Services
{
	public interface IOrderService
	{
		Task<DefaultServiceResponseDto> InsertNewOrderAsync(OrderDto newOrderDto, List<AddOrderItemDto> addOrderItemDtos, string token);
		Task<PaymentsDto> RequestMockApiPaymentsAsync(Domain.Entities.Order orderDb);
		List<OrderDetailsDto> GetUserOrders(OrderFilter filter, int idUser);
		OrderDetailsDto GetOrderDetails(int idOrder, int idUser);
		Task<DefaultServiceResponseDto> ProcessPaymentsNotificationAsync(PaymentsDto paymentsDto);
		Task<DefaultServiceResponseDto> CancelOrderByUserAsync(int userId, int eventId);

	}
}
