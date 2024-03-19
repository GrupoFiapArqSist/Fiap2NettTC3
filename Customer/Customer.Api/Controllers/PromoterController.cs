using Customer.Domain.Dtos.Auth;
using Customer.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Utilities;

namespace Customer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PromoterController : Controller
{
    private readonly IAuthService authService;

    public PromoterController(IAuthService authService)
    {
        this.authService = authService;
    }
    [HttpPost]
    [Route("register")]
    [SwaggerOperation(Summary = "Create a new user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var register = await authService.RegisterAsync(registerDto, StaticUserRoles.PROMOTER);
        return Ok(register);
    }
}