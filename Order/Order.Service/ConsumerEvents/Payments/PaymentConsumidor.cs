using MassTransit;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.Json;
using TicketNow.Domain.Dtos.Payment;

namespace Order.Service.ConsumerEvents.Payments;

public class PaymentConsumidor : IConsumer<ProcessedPaymentDto>
{
    public async Task Consume(ConsumeContext<ProcessedPaymentDto> context)
    {
        var configuration = new ConfigurationBuilder()
                             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory.ToString())
                             .AddJsonFile("appsettings.json")
                             .Build();

        using (var httpClient = new HttpClient())
            try
            {          
                var httpContent = new StringContent(JsonSerializer.Serialize(context.Message),
                System.Text.Encoding.UTF8, "application/json");

                var urlOrderPaymentProcessed = configuration.GetSection("OrderApi")["ProcessPayment"];

                var response = await httpClient.PostAsync(urlOrderPaymentProcessed, httpContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro: {ex.Message}");
            }        
    }
}