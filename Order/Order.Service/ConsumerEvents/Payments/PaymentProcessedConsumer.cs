using MassTransit;
using Microsoft.Extensions.Configuration;
using Order.Domain.Dtos.MockPayment;
using System.Diagnostics;
using System.Text.Json;
namespace Order.Service.ConsumerEvents.Payments
{
    public class PaymentProcessedConsumer : IConsumer<PaymentsDto>
    {
        public Task Consume(ConsumeContext<PaymentsDto> context)
        {
            var configuration = new ConfigurationBuilder()
                                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory.ToString())
                                 .AddJsonFile("appsettings.json")
                                 .Build();

            using (var httpClient = new HttpClient())
                try
                {
                    var httpContent = new StringContent(JsonSerializer.Serialize(context),
                    System.Text.Encoding.UTF8, "application/json");

                    var urlOrderPaymentProcessed = Path.Combine(configuration.GetSection("OrderApi")["UrlBase"], configuration.GetSection("OrderApi")["ProcessPayment"]);

                    var response = httpClient.PostAsync(urlOrderPaymentProcessed, httpContent);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro: {ex.Message}");
                }

            return Task.CompletedTask;
        }
    }
}
