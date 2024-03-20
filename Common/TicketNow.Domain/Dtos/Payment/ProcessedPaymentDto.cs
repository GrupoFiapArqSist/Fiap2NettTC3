using TicketNow.Domain.Enums;

namespace TicketNow.Domain.Dtos.Payment;

public class ProcessedPaymentDto
{
    public string OrderId { get; set; }
    public string PaymentId { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }
}
