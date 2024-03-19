using MassTransit;
using Microsoft.Extensions.Configuration;
using Payment.Domain.DTOs;
using System.Diagnostics;
using System.Text.Json;

namespace Payment.Service.ConsumersEvent.Payments;

public class OrderMadeConsumer : IConsumer<PaymentsDto>
{
    public Task Consume(ConsumeContext<PaymentsDto> context)
    {
        var configuration = new ConfigurationBuilder()
                                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory.ToString())
                                 .AddJsonFile("appsettings.json")
                                 .Build();

        var paymentDto = context.Message;

        Random random = new Random();
        var chance = random.NextDouble();

        if (chance < 0.3)
            paymentDto.PaymentStatus = PaymentStatus.Paid;
        else if (chance > 0.3 && chance < 0.6)
            paymentDto.PaymentStatus = PaymentStatus.Expired;
        else
            paymentDto.PaymentStatus = PaymentStatus.Unauthorized;

        using (var httpClient = new HttpClient())
            try
            {
                var httpContent = new StringContent(JsonSerializer.Serialize(context),
                System.Text.Encoding.UTF8, "application/json");

                var urlOrderPaymentProcessed = Path.Combine(configuration.GetSection("PaymentApi")["UrlBase"], configuration.GetSection("PaymentApi")["ProcessedPayment"]);

                var response = httpClient.PostAsync(urlOrderPaymentProcessed, httpContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro: {ex.Message}");
            }

        return Task.CompletedTask;
    }
}

public class OrderMadeConsumerDefinition : ConsumerDefinition<OrderMadeConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<OrderMadeConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
    }
}