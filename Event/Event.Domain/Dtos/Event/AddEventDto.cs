using Event.Domain.Enums;

namespace Event.Domain.Dtos.Event
{
    public class AddEventDto
    {
        public CategoryEnum Category { get; set; }
        public int PromoterId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string UF { get; set; }
        public string Description { get; set; }
        public string EventTime { get; set; }
        public DateTime EventDate { get; set; }
        public decimal TicketPrice { get; set; }
        public long TicketAmount { get; set; }
    }
}
