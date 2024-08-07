﻿using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Order.Domain.Dtos.Event;
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
using TicketNow.Domain.Dtos.Payment;
using TicketNow.Domain.Enums;
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
    //private readonly IBus _bus;

    public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMapper mapper, IConfiguration configuration,
        IEventIntegration eventIntegration, NotificationContext notificationContext)//, IBus bus)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _orderItemRepository = orderItemRepository;
        _configuration = configuration;
        _eventIntegration = eventIntegration;
        _notificationContext = notificationContext;
        //_bus = bus;
    }
    
    public async Task<List<Domain.Entities.Order>> GetAllOrders()
    {
        var ltOrderDb = (await _orderRepository.SelectAsync())
           .AsQueryable()
           .OrderByDescending(p => p.CreatedAt)
           .ToList();

        return ltOrderDb;
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
        orderDb.TicketPrice = evento.TicketPrice;

        var newOrderDb = await _orderRepository.InsertWithReturnId(orderDb);

        foreach (var itemOrderDto in addOrderItemDtos)
        {
            itemOrderDto.OrderId = newOrderDb.Id;
            _orderItemRepository.Insert(_mapper.Map<Domain.Entities.OrderItem>(itemOrderDto));
        };

        await SendPaymentsToProcessQueueAsync(new PaymentsDto() { OrderId = newOrderDb.Id, PaymentStatus = newOrderDb.PaymentStatus, PaymentMethod = OrderDetailsDto.ReturnPaymentMethodEnum(newOrderDb.PaymentMethod) });

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

    public async Task<DefaultServiceResponseDto> SendPaymentsToProcessQueueAsync(PaymentsDto paymentsDto)
    {
        //var nomeFila = _configuration.GetSection("MassTransit")["OrderProcessedQueue"];
        //var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

        //await endpoint.Send(paymentsDto);

        return new DefaultServiceResponseDto() { Message = StaticNotifications.SendPaymentsToProcessQueueu.Message, Success = true };
    }

    public async Task ProcessPaymentProcessedNotificationAsync(ProcessedPaymentDto processedPaymentDto)
    {
        var orderId = Convert.ToInt32(processedPaymentDto.OrderId.Decrypt(_configuration["EncryptKey"]));
        var paymentId = Convert.ToInt32(processedPaymentDto.PaymentId.Decrypt(_configuration["EncryptKey"]));

        var orderDb = _orderRepository.Select()
            .AsQueryable()
            .Where(db => db.Id == orderId)?.FirstOrDefault();

        if (!orderDb.PaymentStatus.Equals(processedPaymentDto.PaymentStatus))
        {
            orderDb.PaymentStatus = processedPaymentDto.PaymentStatus;
            orderDb.PaymentId = paymentId;
            _orderRepository.Update(orderDb);
            await _orderRepository.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsOrderByEvent(int eventId)
    {
        return await Task.FromResult(_orderRepository.Select()
                     .Any(db => db.EventId == eventId && db.Status == OrderStatusEnum.Active));
    }

}
