namespace Event.Domain.Interfaces.Integration;

public interface IOrderIntegration
{
    Task<bool> ExistsOrderByEvent(int eventId, string token);
}
