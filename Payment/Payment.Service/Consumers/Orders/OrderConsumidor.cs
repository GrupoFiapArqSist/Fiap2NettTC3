using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using TicketNow.Domain.Dtos.Payment;
using TicketNow.Domain.Enums;
using TicketNow.Domain.Extensions;

namespace Payment.Service.Consumers.Orders;

public class OrderConsumidor : IConsumer<PaymentsDto>
{
    public async Task Consume(ConsumeContext<PaymentsDto> context)
    {
        var configuration = new ConfigurationBuilder()
                                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory.ToString())
                                 .AddJsonFile("appsettings.json")
                                 .Build();

        var paymentDto = context.Message;

        Random random = new Random();
        var chance = random.NextDouble();

        if (chance < 0.3)
            paymentDto.PaymentStatus = PaymentStatusEnum.Paid;
        else if (chance > 0.3 && chance < 0.6)
            paymentDto.PaymentStatus = PaymentStatusEnum.Expired;
        else
            paymentDto.PaymentStatus = PaymentStatusEnum.Unauthorized;

        using (var httpClient = new HttpClient())
            try
            {
                var encryptKey = configuration["EncryptKey"];
                var processedPaymentDto = new ProcessedPaymentDto
                {
                    OrderId = context.Message.OrderId.ToString().Encrypt(encryptKey),
                    PaymentMethod = paymentDto.PaymentMethod,
                    PaymentStatus = paymentDto.PaymentStatus
                };

                var httpContent = new StringContent(JsonSerializer.Serialize(processedPaymentDto),
                System.Text.Encoding.UTF8, "application/json");

                var urlOrderPaymentProcessed = configuration.GetSection("PaymentApi")["ProcessedPayment"];

                var response = await httpClient.PostAsync(urlOrderPaymentProcessed, httpContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro: {ex.Message}");
            }        
    }
}