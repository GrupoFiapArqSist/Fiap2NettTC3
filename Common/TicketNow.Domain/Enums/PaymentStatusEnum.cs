using System.ComponentModel;

namespace TicketNow.Domain.Enums;

public enum PaymentStatusEnum
{
    [Description("WaitingPayment")]
    WaitingPayment = 1,
    [Description("Expired")]
    Expired = 2,
    [Description("Paid")]
    Paid = 3,
    [Description("Unauthorized")]
    Unauthorized = 4
}
