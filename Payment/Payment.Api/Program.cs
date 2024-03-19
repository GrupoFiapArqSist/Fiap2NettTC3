using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Payment.Infra.Data.Context;

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
var OrderMadeQueue = builder.Configuration.GetSection("MassTransit:OrderMadeQueue").Get<string>();
var Server = builder.Configuration.GetSection("MassTransit:Server").Get<string>();
var User = builder.Configuration.GetSection("MassTransit:User").Get<string>();
var Password = builder.Configuration.GetSection("MassTransit:Password").Get<string>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Server, "/", h =>
        {
            h.Username(User);
            h.Password(Password);
        });

        cfg.ReceiveEndpoint(OrderMadeQueue, e =>
        {
            e.Consumer<PaymentConsumer>();
        });
        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<PaymentConsumer>();
});
#endregion

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

app.MapPost("/payment/processed", async (ApplicationDbContext _dbContext, HttpRequest _request, PaymentsDto _paymentDto,
                                             IConfiguration configuration, IBus _iBus) =>
{
    _dbContext.Payments.Add(new Payments(_paymentDto.OrderId,
                          _paymentDto.PaymentMethod,
                          _paymentDto.PaymentStatus));

    await _dbContext.SaveChangesAsync();

    var nomeFila = configuration.GetSection("MassTransit")["PaymentsProcessedQueue"];
    var endpoint = await _iBus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));

    await endpoint.Send(_paymentDto);

    return Results.Ok();
});

#endregion