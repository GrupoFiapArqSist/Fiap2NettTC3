using TicketNow.Domain.Filters;

namespace Order.Domain.Filters;

public class OrderFilter : _BaseFilter
{
    public string Name { get; set; }
}
