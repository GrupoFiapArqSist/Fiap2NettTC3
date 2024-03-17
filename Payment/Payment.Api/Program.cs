using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Payment.Api", Description = "Mock Payment Api", Version = "v1" });
    c.OperationFilter<AuthHeaderFilter>();
});

#region [DB]
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("local"));
});
#endregion
builder.Services.AddHostedService<Engine>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock Payment Api"));
}
#region [Migrations and Seeds]
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
#endregion
app.UseHttpsRedirection();


app.Run();

#region [Endpoints]
app.MapGet("/Aplication", async (ApplicationDbContext dbContext, HttpRequest request) =>
{
    var apiKey = request.Headers["API-KEY"];
    if (string.IsNullOrEmpty(apiKey))
    {
        return Results.NotFound("Chave inválida!");
    }
    var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.ApiKey == apiKey);
    return application == null ? Results.NotFound("Aplicação não encontrada!") : Results.Ok(application);
});
app.MapPost("/Application", async (ApplicationDbContext dbContext, CreateApplicationDto applicationDto) =>
{
    if (string.IsNullOrWhiteSpace(applicationDto.Username) ||
        string.IsNullOrWhiteSpace(applicationDto.Password) ||
        string.IsNullOrWhiteSpace(applicationDto.WebhookUrl))
    {
        return Results.BadRequest("Requisição inválida!");
    }
    if (dbContext.Applications.Any(a => a.Username == applicationDto.Username))
    {
        return Results.BadRequest("Aplicação já cadastrada!");
    }
    var entity = new Application(applicationDto.Username, applicationDto.Password, applicationDto.WebhookUrl);
    entity.ApiKey = Guid.NewGuid().ToString().Split("-")[0];
    dbContext.Applications.Add(entity);
    await dbContext.SaveChangesAsync();
    return Results.Ok(entity);
});
app.MapPut("/Application", async (ApplicationDbContext dbContext, UpdateApplicationDto applicationDto) =>
{
    if (string.IsNullOrWhiteSpace(applicationDto.Username) ||
        string.IsNullOrWhiteSpace(applicationDto.Password))
    {
        return Results.BadRequest("Requisição inválida!");
    }
    var application =
        await dbContext.Applications.FirstOrDefaultAsync(a =>
            a.Username == applicationDto.Username &&
            a.Password == applicationDto.Username);
    if (application == null)
    {
        return Results.NotFound("Aplicação não encontrada!");
    }
    application.ApiKey = Guid.NewGuid().ToString().Split("-")[0];
    dbContext.Applications.Update(application);
    await dbContext.SaveChangesAsync();
    return Results.Ok(application);
});
app.MapPost("/Payment", async (ApplicationDbContext dbContext, HttpRequest request, CreatePaymentDto paymentDto) =>
{
    var apiKey = request.Headers["API-KEY"];
    if (string.IsNullOrWhiteSpace(apiKey.ToString()) ||
        paymentDto.OrderId == 0 ||
        paymentDto.PaymentMethod == 0)
    {
        return Results.BadRequest("Requisição inválida!");
    }
    var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.ApiKey == apiKey.ToString());
    if (application == null)
    {
        return Results.NotFound("Aplicação não encontrada!");
    }
    if (dbContext.Payments.Any(p =>
        p.OrderId == paymentDto.OrderId &&
        p.ApplicationId == application.Id))
    {
        return Results.BadRequest("Pedido já Processado!");
    }
    var paymentStatus = paymentDto.PaymentMethod ==
        PaymentMethod.CreditCard ?
        PaymentStatus.Paid :
        PaymentStatus.WaitingPayment;
    if (paymentStatus == PaymentStatus.Paid)
    {
        Random random = new();
        var chance = random.NextDouble();
        if (chance < 0.5)
        {
            paymentStatus = PaymentStatus.Unauthorized;
        }
    }
    Payment entity =
        new(
            application.Id,
            paymentDto.OrderId,
            paymentDto.PaymentMethod,
            paymentStatus);
    dbContext.Payments.Add(entity);
    await dbContext.SaveChangesAsync();
    return Results.Ok(entity);
});
app.MapGet("/Payment/{orderId}", async (ApplicationDbContext dbContext, HttpRequest request, int orderId) =>
{
    var apiKey = request.Headers["API-KEY"];
    if (string.IsNullOrWhiteSpace(apiKey) ||
        orderId == 0)
    {
        return Results.NotFound("Chave inválida!");
    }
    var application = await dbContext.Applications.FirstOrDefaultAsync(x => x.ApiKey == apiKey.ToString());
    var payment =
        await dbContext.Payments
            .FirstOrDefaultAsync(x => x.OrderId == orderId);
    if (payment == null ||
        application == null ||
        payment.ApplicationId != application.Id)
    {
        return Results.NotFound("Pagamento não encontrado!");
    }
    return await Task.FromResult(Results.Ok(payment));
});
#endregion