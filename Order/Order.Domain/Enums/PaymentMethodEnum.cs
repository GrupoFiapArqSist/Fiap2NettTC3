using System.ComponentModel;

namespace Order.Domain.Enums;

public enum PaymentMethodEnum
{
    [Description("Ticket")]
    Ticket = 1,

    [Description("Pix")]
    Pix = 2,

    [Description("CreditCard")]
    CreditCard = 3
}
