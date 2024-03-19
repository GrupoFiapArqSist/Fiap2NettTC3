using AutoMapper;
using Customer.Domain.Dtos.Auth;
using Customer.Domain.Entities;
using Customer.Domain.Interfaces.Services;
using Customer.Service.Validators.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Utilities;
using TicketNow.Infra.CrossCutting.Notifications;
using TicketNow.Service.Services;

namespace Customer.Service.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;

    public AuthService(UserManager<User> userManager,
        IConfiguration configuration,
        IMapper mapper,
        NotificationContext notificationContext)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
        _notificationContext = notificationContext;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var validationResult = Validate(loginDto, Activator.CreateInstance<LoginValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user is null || !user.Active || !user.Approved) { _notificationContext.AddNotification(StaticNotifications.InvalidCredentials); return default; }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordCorrect) { _notificationContext.AddNotification(StaticNotifications.InvalidCredentials); return default; }

        var authClaims = await GetAuthClaims(user);
        var tokenObject = GenerateNewJsonWebToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInHours"],
               out int refreshTokenValidityInHours);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddHours(refreshTokenValidityInHours);
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject),
            RefreshToken = refreshToken,
            Expires = tokenObject.ValidTo,
        };
    }

    public async Task<DefaultServiceResponseDto> RegisterAsync(RegisterDto registerDto, string role = StaticUserRoles.CUSTOMER)
    {
        var validationResult = Validate(registerDto, Activator.CreateInstance<RegisterValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var existsUser = await _userManager.FindByNameAsync(registerDto.Username);
        if (existsUser is not null) { _notificationContext.AddNotification(StaticNotifications.UserAlreadyExists); return default; }

        var newUser = _mapper.Map<User>(registerDto);

        newUser.CreatedAt = DateTime.Now;
        newUser.Active = true;
        newUser.Approved = role != StaticUserRoles.PROMOTER;

        var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = createUserResult.Errors.Select(t => new Notification(t.Code, t.Description));
            _notificationContext.AddNotifications(errors);
            return default;
        }

        await _userManager.AddToRoleAsync(newUser, role);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.UserCreated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> RevokeAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) { _notificationContext.AddNotification(StaticNotifications.UserNotFound); return default; }

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.RevokeToken.Message
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, string userName)
    {
        if (string.IsNullOrWhiteSpace(accessToken) ||
            string.IsNullOrEmpty(refreshToken))
        {
            _notificationContext.AddNotification(StaticNotifications.InvalidToken);
            return default;
        }

        var user = await _userManager.FindByNameAsync(userName);

        if (user is null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            _notificationContext.AddNotification(StaticNotifications.InvalidToken);
            return default;
        }

        var authClaims = await GetAuthClaims(user);
        var tokenObject = GenerateNewJsonWebToken(authClaims);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject),
            RefreshToken = newRefreshToken,
            Expires = tokenObject.ValidTo
        };
    }

    private JwtSecurityToken GenerateNewJsonWebToken(List<Claim> claims)
    {
        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        _ = int.TryParse(_configuration["JWT:TokenValidityInHours"],
            out int tokenValidityInHours);

        return new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(tokenValidityInHours),
            claims: claims,
            signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            );
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<List<Claim>> GetAuthClaims(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };

        foreach (var userRole in userRoles)
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));

        return authClaims;
    }
}
