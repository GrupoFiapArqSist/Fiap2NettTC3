using Event.Domain.Enums;
using Event.Domain.Interfaces.Entities;

namespace Event.Domain.Entities
{
    public class Event : BaseEntity, IEntity<int>
    {
        public int PromoterId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public string Description { get; set; }
        public string EventTime { get; set; }
        public DateTime EventDate { get; set; }
        public CategoryEnum Category { get; set; }
        public decimal TicketPrice { get; set; }
        public long TicketAmount { get; set; }
        public long TicketAvailable { get; set; }
        public bool Active { get; set; }
        public bool Approved { get; set; }

    }
}