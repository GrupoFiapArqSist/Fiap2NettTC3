using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.MockPayment;
using Order.Domain.Dtos.Order;
using Order.Domain.Filters;
using Order.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Extensions;
using TicketNow.Infra.CrossCutting.Notifications;

namespace Order.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : Controller
{
	private readonly IOrderService _orderService;
	private readonly IMapper _mapper;

	public OrderController(IOrderService orderService, IMapper mapper)
	{
		_orderService = orderService;
		_mapper = mapper;
	}

	[HttpPost]
	[SwaggerOperation(Summary = "Create a new order")]
	[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
	public async Task<IActionResult> NewOrder([FromBody] AddOrderDto AddOrderDto)
	{
		var newOrderDto = _mapper.Map<OrderDto>(AddOrderDto);

		newOrderDto.UserId = this.GetUserIdLogged();
		var accessToken = this.GetAccessToken();
		var response = await _orderService.InsertNewOrderAsync(newOrderDto, AddOrderDto.AddOrderItemDto, accessToken);
		return Ok(response);

	}

	[HttpGet]
	[SwaggerOperation(Summary = "Get orders by id user")]
	[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<OrderDetailsDto>))]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
	public IActionResult GetOrders([FromQuery] OrderFilter filter)
	{
		var ltOrder = _orderService.GetUserOrders(filter, this.GetUserIdLogged());
		if (ltOrder is null || ltOrder.Count.Equals(0)) return NotFound(new DefaultServiceResponseDto() { Message = StaticNotifications.OrderNotFound.Message, Success = true });

		return Ok(ltOrder);
	}

	[HttpGet("get-order-detail")]
	[SwaggerOperation(Summary = "Get order details by id")]
	[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(OrderDetailsDto))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, Type = typeof(DefaultServiceResponseDto))]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
	public IActionResult GetOrderDetail(int idOrder)
	{
		var orderDetail = _orderService.GetOrderDetails(idOrder, this.GetUserIdLogged());
		if (orderDetail is null) return NotFound(new DefaultServiceResponseDto() { Message = StaticNotifications.OrderNotFound.Message, Success = false });

		return Ok(orderDetail);
	}

    [HttpDelete("{id}")]
	[SwaggerOperation(Summary = "Cancel order by user")]
	[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
	[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
	public async Task<IActionResult> CancelOrder(int id)
	{
		return Ok(await _orderService.CancelOrderByUserAsync(this.GetUserIdLogged(), id));
	}

    [HttpPost]
    [AllowAnonymous]
    [Route("/payments/process")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task ReceivePaymentsProcessedNotification([FromBody] PaymentsDto paymentsDto)
    {
        await _orderService.ProcessPaymentsProcessedNotificationAsync(paymentsDto);
    }

    [HttpGet("get-order-active-event")]
    [SwaggerOperation(Summary = "Verify order active on event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public IActionResult GetOrderActiveOnEvent(int idEvent)
    {
        return Ok(_orderService.GetOrderActiveOnEvent(idEvent));
    }
}