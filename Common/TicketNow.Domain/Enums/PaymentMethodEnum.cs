using System.ComponentModel;

namespace TicketNow.Domain.Enums;

public enum PaymentMethodEnum
{
    [Description("Ticket")]
    Ticket = 1,

    [Description("Pix")]
    Pix = 2,

    [Description("CreditCard")]
    CreditCard = 3
}
