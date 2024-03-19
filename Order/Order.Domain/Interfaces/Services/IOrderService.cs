using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.Payment;
using Order.Domain.Dtos.Order;
using Order.Domain.Filters;
using TicketNow.Domain.Dtos.Default;

namespace Order.Domain.Interfaces.Services;

public interface IOrderService
{
    Task<DefaultServiceResponseDto> InsertNewOrderAsync(OrderDto newOrderDto, List<AddOrderItemDto> addOrderItemDtos, string token);
    List<OrderDetailsDto> GetUserOrders(OrderFilter filter, int idUser);
    OrderDetailsDto GetOrderDetails(int idOrder, int idUser);
    Task<DefaultServiceResponseDto> CancelOrderByUserAsync(int userId, int eventId);
    Task<DefaultServiceResponseDto> SendPaymentsToProcessQueueuAsync(PaymentsDto paymentsDto);
    Task ProcessPaymentsProcessedNotificationAsync(PaymentsDto paymentsDto);
    Task<bool> ExistsOrderByEvent(int idEvent);
}
