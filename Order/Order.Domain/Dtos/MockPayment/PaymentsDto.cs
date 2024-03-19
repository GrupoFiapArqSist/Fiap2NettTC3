﻿using Order.Domain.Enums;

namespace Order.Domain.Dtos.MockPayment;

public class PaymentsDto
{
	public int OrderId { get; set; }
	public PaymentMethodEnum PaymentMethod { get; set; }
	public PaymentStatusEnum PaymentStatus { get; set; }
}