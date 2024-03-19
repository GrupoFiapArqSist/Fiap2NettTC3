namespace Payment.Domain.DTOs;

public class PaymentsDto
{
    public PaymentsDto()
    {
    }

    public PaymentsDto(int orderId, PaymentMethod paymentMethod, PaymentStatus paymentStatus)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
    }

    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}