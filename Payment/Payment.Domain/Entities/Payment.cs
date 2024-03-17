public class Payment
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Application Application { get; set; }

    public Payment(int applicationId, int orderId, PaymentMethod paymentMethod, PaymentStatus paymentStatus)
    {
        ApplicationId = applicationId;
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        PaymentStatus = paymentStatus;
        CreatedAt = DateTime.Now;
    }
}
