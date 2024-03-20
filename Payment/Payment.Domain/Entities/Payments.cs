using TicketNow.Domain.Entities;
using TicketNow.Domain.Enums;
using TicketNow.Domain.Interfaces.Entities;

public class Payments : BaseEntity, IEntity<int>
{
    public int OrderId { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }

    public Payments()
    {
            
    }

    public Payments(int orderId, PaymentMethodEnum paymentMethod, PaymentStatusEnum paymentStatus)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        CreatedAt = DateTime.Now;
    }
}
