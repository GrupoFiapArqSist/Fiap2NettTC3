﻿namespace Payment.Domain.DTOs;

public class PaymentsDto
{
    public int OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}