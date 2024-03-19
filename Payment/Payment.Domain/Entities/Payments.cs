using TicketNow.Domain.Entities;
using TicketNow.Domain.Interfaces.Entities;

public class Payments : BaseEntity, IEntity<int>
{
    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }

    public Payments()
    {
            
    }

    public Payments(int orderId, PaymentMethod paymentMethod, PaymentStatus paymentStatus)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
    }
}
