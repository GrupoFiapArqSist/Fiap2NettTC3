using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Customer.Domain.Filters.Extensions;

public static class ControllerExtensions
{
    public static int GetUserIdLogged(this Controller controller)
    {
        var identity = (ClaimsIdentity)controller.User.Identity;
        IEnumerable<Claim> claims = identity.Claims;

        return Convert.ToInt32(claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault());
    }

    public static string GetAccessToken(this Controller controller)
    {
        var token = controller.HttpContext.GetTokenAsync("access_token").Result;

        if (string.IsNullOrEmpty(token))
        {
            if (controller.Request.Headers.Any(r => r.Key == "Authorization"))
                return controller.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
        }

        return token;
    }
}
