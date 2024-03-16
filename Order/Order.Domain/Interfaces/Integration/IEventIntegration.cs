using Order.Domain.Dtos.Event;

namespace Order.Domain.Interfaces.Integration;
public interface IEventIntegration
{
	Task<EventDto> GetEventById(int eventId, string accessToken);
}
