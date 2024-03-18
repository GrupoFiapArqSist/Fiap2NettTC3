using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.MockPayment;
using Order.Domain.Dtos.Order;
using Order.Domain.Enums;
using Order.Domain.Filters;
using Order.Domain.Interfaces.Integration;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Services;
using Order.Service.Validators.Event;
using Order.Service.Validators.Order;
using System.Linq.Dynamic.Core;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Extensions;
using TicketNow.Infra.CrossCutting.Notifications;
using TicketNow.Service.Services;

namespace Order.Service.Services;

public class OrderService : BaseService, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IEventIntegration _eventIntegration;
    private readonly NotificationContext _notificationContext;
    private readonly IBus _bus;

    public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper, IConfiguration configuration,
        IEventIntegration eventIntegration, NotificationContext notificationContext, IBus bus)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _orderItemRepository = orderItemRepository;
        _configuration = configuration;
        _eventIntegration = eventIntegration;
        _notificationContext = notificationContext;
        _bus = bus;
    }

    public async Task<DefaultServiceResponseDto> InsertNewOrderAsync(OrderDto newOrderDto, List<AddOrderItemDto> addOrderItemDtos, string accessToken)
    {
        newOrderDto.Tickets = addOrderItemDtos.Count;

        var validationResult = Validate(newOrderDto, Activator.CreateInstance<AddOrderValidator>());
        if (!validationResult.IsValid)
        {
            _notificationContext.AddNotifications(validationResult.Errors);
            return default(DefaultServiceResponseDto);
        }

        foreach (var item in addOrderItemDtos)
        {
            validationResult = Validate(item, Activator.CreateInstance<AddOrderItemValidator>());
            if (!validationResult.IsValid)
            {
                _notificationContext.AddNotifications(validationResult.Errors);
                return default(DefaultServiceResponseDto);
            }
        }

        var evento = await _eventIntegration.GetEventById(newOrderDto.EventId, accessToken);

        var orderDb = _mapper.Map<Domain.Entities.Order>(newOrderDto);
        orderDb.CreatedAt = DateTime.Now;
        orderDb.Status = OrderStatusEnum.Active;
        orderDb.PaymentStatus = PaymentStatusEnum.WaitingPayment;
        orderDb.Price = evento.TicketPrice * newOrderDto.Tickets;

        var newOrderDb = await _orderRepository.InsertWithReturnId(orderDb);

        foreach (var itemOrderDto in addOrderItemDtos)
        {
            itemOrderDto.OrderId = newOrderDb.Id;
            _orderItemRepository.Insert(_mapper.Map<Domain.Entities.OrderItem>(itemOrderDto));
        };

        await SendPaymentsToProcessQueueuAsync(new PaymentsDto() { OrderId = newOrderDb.Id, PaymentStatus = newOrderDb.PaymentStatus, PaymentMethod = OrderDetailsDto.ReturnPaymentMethodEnum(newOrderDb.PaymentMethod) });

        if (newOrderDb.Id > 0)
            return new DefaultServiceResponseDto()
            {
                Message = StaticNotifications.OrderSucessWaitingPayment.Message,
                Success = true
            };
        else
            return new DefaultServiceResponseDto()
            {
                Message = StaticNotifications.OrderError.Message,
                Success = true
            };
    }

    public List<OrderDetailsDto> GetUserOrders(OrderFilter filter, int idUser)
    {
        var ltOrderDb = _orderRepository.Select()
           .AsQueryable()
           .OrderByDescending(p => p.CreatedAt)
           .ApplyFilter(filter)
           .Where(db => db.UserId.Equals(idUser)).ToList();

        List<OrderDetailsDto> ltOrderDetails = new();
        ltOrderDb.ForEach(db => ltOrderDetails.Add(new OrderDetailsDto(db.Id, db.EventId, db.Status, db.PaymentStatus,
            db.PaymentMethod, db.Tickets, db.Price, _mapper.Map<List<OrderItemDto>>(db.OrderItens))));

        return ltOrderDetails;
    }

    public OrderDetailsDto GetOrderDetails(int idOrder, int idUser)
    {
        var orderDb = _orderRepository.Select()
            .AsQueryable()
            .Where(db => db.Id.Equals(idOrder) && db.UserId.Equals(idUser))?.FirstOrDefault();

        OrderDetailsDto orderDetailsDto = new(orderDb.Id, orderDb.EventId, orderDb.Status, orderDb.PaymentStatus,
            orderDb.PaymentMethod, orderDb.Tickets, orderDb.Price, _mapper.Map<List<OrderItemDto>>(orderDb.OrderItens));

        return orderDetailsDto;
    }

    public async Task<DefaultServiceResponseDto> CancelOrderByUserAsync(int userId, int orderId)
    {
        var orderDb = _orderRepository.Select().Where(db => db.Id.Equals(orderId))?.FirstOrDefault();

        if (orderDb is null) return new DefaultServiceResponseDto() { Message = StaticNotifications.CancelOrderByUserOrderNotFound.Message, Success = false };
        if (orderDb.Status.Equals(OrderStatusEnum.Canceled)) return new DefaultServiceResponseDto() { Message = StaticNotifications.CancelOrderByUserOrderAlreadyCanceled.Message, Success = false };

        orderDb.Status = OrderStatusEnum.Canceled;

        _orderRepository.Update(orderDb);
        await _orderRepository.SaveChangesAsync();

        return new DefaultServiceResponseDto() { Message = StaticNotifications.OrderCanceledSucess.Message, Success = true };
    }

    public async Task<DefaultServiceResponseDto> SendPaymentsToProcessQueueuAsync(PaymentsDto paymentsDto)
    {
        var nomeFila = _configuration.GetSection("MassTransit")["OrderMade"];
        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

        await endpoint.Send(paymentsDto);

        return new DefaultServiceResponseDto() { Message = StaticNotifications.SendPaymentsToProcessQueueu.Message, Success = true };
    }

    public async Task ProcessPaymentsProcessedNotificationAsync(PaymentsDto paymentsDto)
    {
        var orderDb = _orderRepository.Select()
            .AsQueryable()
            .Where(db => db.Id.Equals(paymentsDto.OrderId))?.FirstOrDefault();

        if (!orderDb.PaymentStatus.Equals(paymentsDto.PaymentStatus))
        {
            orderDb.PaymentStatus = paymentsDto.PaymentStatus;
            _orderRepository.Update(orderDb);
            await _orderRepository.SaveChangesAsync();
        }
    }

    public DefaultServiceResponseDto GetOrderActiveOnEvent(int id)
    {
        var orderDb = _orderRepository.Select()
            .AsQueryable()
            .Where(db => db.EventId.Equals(id) && db.Status.Equals(OrderStatusEnum.Active));

        if(orderDb is not null && orderDb.Any())
            return new DefaultServiceResponseDto() { Message = StaticNotifications.EventContainsOrderActive.Message, Success = true };
        else
            return new DefaultServiceResponseDto() { Message = StaticNotifications.EventDontContainsOrderActive.Message, Success = true };
    }

}
