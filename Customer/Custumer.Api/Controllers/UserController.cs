using Customer.Domain.Dtos.Default;
using Customer.Domain.Dtos.User;
using Customer.Domain.Filters;
using Customer.Domain.Interfaces.Services;
using Customer.Domain.Utilities;
using Custumer.Domain.Filters.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Customer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get all users")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IList<UserResponseDto>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public IActionResult GetAll([FromQuery] UserFilter filter)
    {
        var users = _userService.GetAll(filter);
        if (users is null)
            return NotFound();

        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = StaticUserRoles.ADMIN)]
    [SwaggerOperation(Summary = "Get user by id")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.CUSTOMER)]
    [SwaggerOperation(Summary = "Update user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
    {
        var response = await _userService.Update(updateUserDto, this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpPut("Password")]
    [Authorize]
    [SwaggerOperation(Summary = "Change password")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
    {
        var response = await _userService.UpdatePassword(updateUserPasswordDto, this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpPost("UploadPhoto")]
    [Authorize(Roles = StaticUserRoles.CUSTOMER)]
    [SwaggerOperation(Summary = "Upload photo")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<dynamic>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        var response = await _userService.UploadPhoto(file, this.GetUserIdLogged());
        return Ok(response);
    }

    [HttpDelete]
    [Authorize(Roles = StaticUserRoles.CUSTOMER)]
    [SwaggerOperation(Summary = "Delete user")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Delete()
    {
        var response = await _userService.Delete(this.GetUserIdLogged());
        return Ok(response);
    }
}
