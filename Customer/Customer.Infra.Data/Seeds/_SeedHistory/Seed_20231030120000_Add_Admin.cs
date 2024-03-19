using Customer.Domain.Interfaces.Services;
using Customer.Domain.Dtos.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketNow.Domain.Entities;
using TicketNow.Domain.Utilities;

namespace Customer.Infra.Data.Seeds._SeedHistory;

public class Seed_20231030120000_Add_Admin : Seed
{
    private readonly IAuthService authService;

    public Seed_20231030120000_Add_Admin(
        DbContext dbContext,
        IServiceProvider serviceProvider) : base(dbContext)
    {
        authService = serviceProvider.CreateScope().ServiceProvider.GetService<IAuthService>();
    }

    public override void Up()
    {
        var user = new RegisterDto
        {
            FirstName = "Admin",
            DocumentType = Domain.Enums.DocumentTypeEnum.CNPJ,
            Document = "00000000000000",
            Email = "admin@gmail.com",
            Password = "1q2w3e4r@#$A",
            LastName = "Admin",
            Username = "admin",
        };

        authService.RegisterAsync(user, StaticUserRoles.ADMIN).Wait();
    }
}
