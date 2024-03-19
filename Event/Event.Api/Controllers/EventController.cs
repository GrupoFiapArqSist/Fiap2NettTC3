using Event.Domain.Dtos.Event;
using Event.Domain.Filters;
using Event.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Infra.CrossCutting.Notifications;
using TicketNow.Domain.Extensions;
using TicketNow.Domain.Utilities;

namespace Event.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : Controller
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Add event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Add([FromBody] AddEventDto addEventDto)
    {
        var addResult = await _eventService.AddEventAsync(addEventDto, this.GetUserIdLogged());
        return Ok(addResult);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Update event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Put([FromBody] UpdateEventDto updateEventDto)
    {
        var updateResult = await _eventService.UpdateEventAsync(updateEventDto, this.GetUserIdLogged());
        return Ok(updateResult);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get event by id")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(EventDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public IActionResult GetById(int id)
    {
        var eventResult = _eventService.GetEvent(id);
        if (eventResult is null)
            return NotFound();

        return Ok(eventResult);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all events")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<EventDto>))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public IActionResult GetAll([FromQuery] EventFilter filter)
    {
        var events = _eventService.GetAllEvents(filter, true);
        if (events is null)
            return NotFound();

        return Ok(events);
    }

    [HttpGet("GetPendingEvents")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get all events pending")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<EventDto>))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public IActionResult GetAllPending([FromQuery] EventFilter filter)
    {
        var events = _eventService.GetAllEvents(filter, false);
        if (events is null)
            return NotFound();

        return Ok(events);
    }

    [HttpGet("GetByPromoter")]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Get all events by promoter")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ICollection<EventDto>))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public IActionResult GetAllByPromoter([FromQuery] EventFilter filter)
    {
        var events = _eventService.GetAllEventsByPromoter(filter, this.GetUserIdLogged());
        if (events is null)
            return NotFound();

        return Ok(events);
    }

    [HttpPut("DisableById/{id}")]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Disable event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DisableEvent(int id)
    {
        var disableResult = await _eventService.SetState(id, false, this.GetUserIdLogged());
        return Ok(disableResult);
    }

    [HttpPut("EnableById/{id}")]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Enable event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> EnableEvent(int id)
    {
        var enableResult = await _eventService.SetState(id, true, this.GetUserIdLogged());
        return Ok(enableResult);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = StaticUserRoles.PROMOTER)]
    [SwaggerOperation(Summary = "Delete event by id")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var token = this.GetAccessToken();
        var deleteResult = await _eventService.DeleteEvent(id, this.GetUserIdLogged(), token);
        return Ok(deleteResult);
    }

    [HttpPut("ApproveById/{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Approve event")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> ApproveById(int id)
    {
        var approveResult = await _eventService.Approve(id);
        return Ok(approveResult);
    }

    [HttpPut("get-event-active-inative/{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get event active or inative")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public IActionResult GetEventActiveOrInative(int id)
    {
        return Ok(_eventService.GetEventActive(id));
    }
}


