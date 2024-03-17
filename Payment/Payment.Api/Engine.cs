

using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

public class Engine : BackgroundService, IHostedService
{
    private readonly TimeSpan period = TimeSpan.FromMinutes(1);
    private readonly IDbContextFactory<ApplicationDbContext> context;

    public Engine(IDbContextFactory<ApplicationDbContext> context)
    {
        this.context = context;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            using (ApplicationDbContext dbContext = context.CreateDbContext())
            {
                await ProcessPayments(dbContext);
            }
        }
    }

    private async Task ProcessPayments(ApplicationDbContext dbContext)
    {
        Debug.WriteLine("Executando processamento");
        List<Payment> paymentsToByNotified = new();
        var paymentsToProcess = await dbContext.Payments
            .Include(x => x.Application)
            .Where(x => x.PaymentStatus == PaymentStatus.WaitingPayment)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
        paymentsToProcess.ForEach(payment =>
        {
            Random random = new();
            payment.PaymentStatus = random.NextDouble() > 0.5 ? PaymentStatus.Paid : PaymentStatus.Expired;
            dbContext.Payments.Update(payment);
            paymentsToByNotified.Add(payment);
        });
        await dbContext.SaveChangesAsync();
        await NotifyPayments(paymentsToByNotified);
    }

    private async Task NotifyPayments(List<Payment> payments)
    {
        using (HttpClient client = new())
        {
            payments.ForEach(async payment =>
            {
                try
                {
                    var content = new StringContent(JsonSerializer.Serialize(payment), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(payment.Application.WebhookUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Erro ao notificar pagamento: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro: {ex.Message}");
                }
            });
        }
    }
}