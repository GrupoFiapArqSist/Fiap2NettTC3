using AutoMapper;
using Event.Domain.Dtos.Event;
using Event.Domain.Filters;
using Event.Domain.Interfaces.Repositories;
using Event.Domain.Interfaces.Services;
using Event.Service.Validators.Event;
using System.Linq.Dynamic.Core;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Extensions;
using TicketNow.Infra.CrossCutting.Notifications;
using TicketNow.Service.Services;

namespace Event.Service.Services;

public class EventService : BaseService, IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;

    public EventService(IEventRepository eventRepository, IMapper mapper, NotificationContext notificationContext)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _notificationContext = notificationContext;
    }

    public async Task<DefaultServiceResponseDto> AddEventAsync(AddEventDto addEventDto, int promoterId)
    {
        addEventDto.PromoterId = promoterId;
        var validationResult = Validate(addEventDto, Activator.CreateInstance<AddEventValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var entity = _mapper.Map<Domain.Entities.Event>(addEventDto);

        if (await _eventRepository.ExistsByName(entity.Name) is not null)
        {
            _notificationContext.AddNotification(StaticNotifications.EventAlreadyExists); return default;
        }

        entity.CreatedAt = DateTime.Now;
        entity.Active = true;
        entity.TicketAvailable = addEventDto.TicketAmount;

        _eventRepository.Insert(entity);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.EventCreated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> UpdateEventAsync(UpdateEventDto updateEventDto, int promoterId)
    {
        updateEventDto.PromoterId = promoterId;
        var validationResult = Validate(updateEventDto, Activator.CreateInstance<UpdateEventValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; };

        if (!await _eventRepository.ExistsByEventIdAndPromoterId(updateEventDto.Id, promoterId))
        { _notificationContext.AddNotification(StaticNotifications.EventNotFound); return default; };

        var entity = _mapper.Map<Domain.Entities.Event>(updateEventDto);
        _eventRepository.Update(entity);
        await _eventRepository.SaveChangesAsync();

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = string.Format(StaticNotifications.EventUpdated.Message)
        };
    }

    public EventDto GetEvent(int eventId)
    {
        var eventDescription = _eventRepository.Select(eventId);

        return _mapper.Map<EventDto>(eventDescription);
    }

    public ICollection<EventDto> GetAllEvents(EventFilter filter, bool approved)
    {
        var events = _eventRepository
           .Select()
           .AsQueryable()
           .OrderByDescending(p => p.CreatedAt)
           .ApplyFilter(filter)
           .Where(p => p.Approved == approved);

        if (filter.Name is not null)
            events = events.Where(t => t.Name == filter.Name);

        return _mapper.Map<List<EventDto>>(events);
    }

    public ICollection<EventDto> GetAllEventsByPromoter(EventFilter filter, int promoterId)
    {
        if (promoterId == 0)
        {
            _notificationContext.AddNotification(StaticNotifications.InvalidPromoter);
            return default;
        }

        var events = _eventRepository
           .Select()
           .AsQueryable()
           .OrderByDescending(p => p.CreatedAt)
           .ApplyFilter(filter)
           .Where(events => events.PromoterId == promoterId);

        if (filter.Name is not null)
            events = events.Where(t => t.Name == filter.Name);

        return _mapper.Map<List<EventDto>>(events);
    }

    public async Task<DefaultServiceResponseDto> SetState(int eventId, bool active, int promoterId)
    {
        var eventResult = await _eventRepository.SelectByIds(eventId, promoterId);

        if (eventResult is null) { _notificationContext.AddNotification(StaticNotifications.EventNotFound); return default; };

        if (eventResult.Active == active)
        {
            _notificationContext.AddNotification(StaticNotifications.EventAlreadyActiveOrInactive.Key,
                                                 string.Format(StaticNotifications.EventAlreadyActiveOrInactive.Message,
                                                 active ?
                                                 "ativo" :
                                                 "inativo"));
            return default;
        };

        eventResult.Active = active;

        _eventRepository.Update(eventResult);
        await _eventRepository.SaveChangesAsync();

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = string.Format(StaticNotifications.EventState.Message,
                                                 active ?
                                                 "ativado" :
                                                 "inativado")
        };
    }

    public async Task<DefaultServiceResponseDto> DeleteEvent(int eventId, int promoterId)
    {
        var eventResult = await _eventRepository.SelectByIds(eventId, promoterId);

        //var orderResult = _orderRepository
        //    .Select()
        //    .Where(r => r.EventId == eventId && r.UserId == promoterId)
        //    .FirstOrDefault();

        //if (orderResult is not null) { _notificationContext.AddNotification(StaticNotifications.EventDeletedConflict); return default; };

        if (eventResult is null) { _notificationContext.AddNotification(StaticNotifications.EventNotFound); return default; };

        _eventRepository.Delete(eventResult.Id);
        await _eventRepository.SaveChangesAsync();

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = string.Format(StaticNotifications.EventDeleted.Message)
        };
    }

    public async Task<DefaultServiceResponseDto> Approve(int eventId)
    {
        var eventResult = _eventRepository.Select(eventId);

        if (eventResult is null) { _notificationContext.AddNotification(StaticNotifications.EventNotFound); return default; };

        eventResult.Approved = true;

        _eventRepository.Update(eventResult);
        await _eventRepository.SaveChangesAsync();

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.EventApproved.Message
        };
    }

    public DefaultServiceResponseDto GetEventActive(int eventId)
    {
        var eventDb = _eventRepository
            .Select()
            .Where(db => db.Id.Equals(eventId) && db.Active);

        if (eventDb is not null)
            return new DefaultServiceResponseDto
            {
                Success = true,
                Message = string.Format(StaticNotifications.EventAlreadyActiveOrInactive.Message, "ativo")
            };
        else
            return new DefaultServiceResponseDto
            {
                Success = true,
                Message = string.Format(StaticNotifications.EventAlreadyActiveOrInactive.Message, "inativo")
            };
    }
}
