using Customer.Domain.Dtos.Auth;
using TicketNow.Domain.Dtos.Default;
using TicketNow.Domain.Utilities;

namespace Customer.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<DefaultServiceResponseDto> RegisterAsync(RegisterDto registerDto, string role = StaticUserRoles.CUSTOMER);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<DefaultServiceResponseDto> RevokeAsync(string userName);
    Task<LoginResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, string userName);
}
