using Microsoft.EntityFrameworkCore;
using Event.Domain.Interfaces.Repositories;
using Event.Infra.Data.Context;

namespace Event.Infra.Data.Repositories
{
    public class EventRepository : BaseRepository<Domain.Entities.Event, int, ApplicationDbContext>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByEventIdAndPromoterId(int idEvent, int promoterId)
        {
            return await _dataContext.Set<Domain.Entities.Event>().AnyAsync(t => t.Id == idEvent &&
                                                     t.PromoterId == promoterId);
        }

        public async Task<Domain.Entities.Event> ExistsByName(string name)
        {
            return await _dataContext.Set<Domain.Entities.Event>().FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<Domain.Entities.Event> SelectByIds(int idEvent, int promoterId)
        {
            return await _dataContext.Set<Domain.Entities.Event>().FirstOrDefaultAsync(t => t.Id == idEvent && t.PromoterId == promoterId);
        }
    }
}
