using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Order.Api.Filters;
using Order.Api.Mapper;
using Order.Domain.Interfaces.Integration;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.Services;
using Order.Infra.Data.Context;
using Order.Infra.Data.Repositories;
using Order.Service.ConsumerEvents.Payments;
using Order.Service.Integration;
using Order.Service.Services;
using System.Text;
using System.Text.Json.Serialization;
using TicketNow.Domain.Interfaces.Services;
using TicketNow.Infra.CrossCutting.Notifications;
using TicketNow.Service.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

#region [DB]
services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("local");
    options.UseSqlServer(connectionString);
});
#endregion

#region [Authentication]
services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });
#endregion

#region [Mapper]            
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
services.AddSingleton(mapper);
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region [DI]
services.AddScoped<NotificationContext>();
services.AddScoped<IBaseService, BaseService>();
services.AddScoped<IOrderService, OrderService>();

services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderItemRepository, OrderItemRepository>();

services.AddScoped<IEventIntegration, EventIntegration>();
#endregion

#region [Swagger]            
services.AddSwaggerGen();
services.AddCors();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order v1", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Auth.
                                    Ex: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});
#endregion

services.AddControllers(options =>
{
    options.Filters.Add<NotificationFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

#region [MassTransit]
var paymentProcessedQueue = builder.Configuration.GetSection("MassTransit")["PaymentProcessedQueue"] ?? String.Empty;
var Server = builder.Configuration.GetSection("MassTransit")["Server"];
var User = builder.Configuration.GetSection("MassTransit")["User"];
var Password = builder.Configuration.GetSection("MassTransit")["Password"];

builder.Services.AddMassTransit((x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(Server, "/", h =>
        {
            h.Username(User);
            h.Password(Password);
        });
        
        cfg.ConfigureEndpoints(context);

        cfg.ReceiveEndpoint(paymentProcessedQueue, e =>
        {
            e.Consumer<PaymentConsumidor>();
        });
    });

    x.AddConsumer<PaymentConsumidor>();

}));

#endregion

var app = builder.Build();

#region [Migrations and Seeds]
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
    dbContext.EnsureSeedData(scope.ServiceProvider);
}
#endregion

#region [Swagger App]            
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order v1");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    });
}
#endregion

#region [Cors]            
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
#endregion

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();