namespace Event.Domain.Interfaces.Repositories;
public interface IEventRepository : IBaseRepository<Domain.Entities.Event, int>
{
    Task<Domain.Entities.Event> ExistsByName(string name);
    Task<Domain.Entities.Event> SelectByIds(int idEvent, int promoterId);
    Task<bool> ExistsByEventIdAndPromoterId(int idEvent, int promoterId);
}
