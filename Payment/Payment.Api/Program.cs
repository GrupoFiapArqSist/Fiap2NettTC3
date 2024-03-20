using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Payment.Infra.Data.Context;
using Payment.Service.Consumers.Orders;
using TicketNow.Domain.Dtos.Payment;
using TicketNow.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Payment.Api", Description = "Payment Api", Version = "v1" });
    c.OperationFilter<AuthHeaderFilter>();
});

#region [DB]
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("local"));
});
#endregion

#region [MassTransit]
var orderProcessedQueue = builder.Configuration.GetSection("MassTransit:OrderProcessedQueue").Get<string>();
var server = builder.Configuration.GetSection("MassTransit:Server").Get<string>();
var user = builder.Configuration.GetSection("MassTransit:User").Get<string>();
var password = builder.Configuration.GetSection("MassTransit:Password").Get<string>();

builder.Services.AddMassTransit((x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(server, "/", h =>
        {
            h.Username(user);
            h.Password(password);
        });

        cfg.ReceiveEndpoint(orderProcessedQueue, e =>
        {
            e.Consumer<OrderConsumidor>();
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<OrderConsumidor>();
}));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Api"));
}

#region [Migrations and Seeds]
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
#endregion

app.UseHttpsRedirection();

#region [Endpoints]

app.MapPost("/payment/processed", async (ApplicationDbContext _dbContext, HttpRequest _request, ProcessedPaymentDto _processedPaymentDto,
                                             IConfiguration configuration, IBus _iBus) =>
{
    var orderId = Convert.ToInt32(_processedPaymentDto.OrderId.Decrypt(configuration["EncryptKey"]));
    var entity = new Payments(orderId,
                          _processedPaymentDto.PaymentMethod,
                          _processedPaymentDto.PaymentStatus
                          );
    _dbContext.Payments.Add(entity);
    await _dbContext.SaveChangesAsync();

    _processedPaymentDto.PaymentId = entity.Id.ToString().Encrypt(configuration["EncryptKey"]);
    var queueName = configuration.GetSection("MassTransit")["PaymentProcessedQueue"];
    var endpoint = await _iBus.GetSendEndpoint(new Uri($"queue:{queueName}"));

    await endpoint.Send(_processedPaymentDto);

    return Results.Ok();
});

#endregion

app.Run();
