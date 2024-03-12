using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Event.Domain.Entities;
using Event.Domain.Interfaces.Services;
using Event.Domain.Utilities;

namespace Event.Infra.Data.Seeds._SeedHistory
{
    // ajustar
    //public class Seed_20231030120000_Add_Admin : Seed
    //{
    //    private readonly IAuthService authService;

    //    public Seed_20231030120000_Add_Admin(
    //        DbContext dbContext,
    //        IServiceProvider serviceProvider) : base(dbContext)
    //    {
    //        authService = serviceProvider.CreateScope().ServiceProvider.GetService<IAuthService>();
    //    }

    //    public override void Up()
    //    {
    //        var user = new RegisterDto
    //        {
    //            FirstName = "Admin",
    //            DocumentType = Domain.Enums.DocumentTypeEnum.CNPJ,
    //            Document = "00000000000000",
    //            Email = "admin@gmail.com",
    //            Password = "1q2w3e4r@#$A",
    //            LastName = "Admin",
    //            Username = "admin",
    //        };

    //        authService.RegisterAsync(user, StaticUserRoles.ADMIN).Wait();
    //    }
    //}
}
