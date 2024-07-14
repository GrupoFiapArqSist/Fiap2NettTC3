using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.Order;
using Order.Domain.Filters;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Dtos.Payment;

namespace Order.Domain.Interfaces.Services;

public interface IOrderService
{
    Task<DefaultServiceResponseDto> InsertNewOrderAsync(OrderDto newOrderDto, List<AddOrderItemDto> addOrderItemDtos, string token);
    Task<List<Domain.Entities.Order>> GetAllOrders();
    List<OrderDetailsDto> GetUserOrders(OrderFilter filter, int idUser);
    OrderDetailsDto GetOrderDetails(int idOrder, int idUser);
    Task<DefaultServiceResponseDto> CancelOrderByUserAsync(int userId, int eventId);
    Task<DefaultServiceResponseDto> SendPaymentsToProcessQueueAsync(PaymentsDto paymentsDto);
    Task ProcessPaymentProcessedNotificationAsync(ProcessedPaymentDto processedPaymentDto);
    Task<bool> ExistsOrderByEvent(int idEvent);
}
