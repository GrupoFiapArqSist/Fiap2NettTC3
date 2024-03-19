using Customer.Domain.Dtos.User;
using Customer.Domain.Filters;
using Microsoft.AspNetCore.Http;
using TicketNow.Domain.Dtos.Default;

namespace Customer.Domain.Interfaces.Services;

public interface IUserService
{
    ICollection<UserResponseDto> GetAll(UserFilter filter);
    UserResponseDto GetById(int id);
    Task<DefaultServiceResponseDto> Update(UpdateUserDto updateUserDto, int id);
    Task<DefaultServiceResponseDto> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto, int id);
    Task<DefaultServiceResponseDto> UploadPhoto(IFormFile file, int id);
    Task<DefaultServiceResponseDto> Delete(int id);
    Task<DefaultServiceResponseDto> ActivateAsync(ActivateUserDto activateUserDto);
    Task<DefaultServiceResponseDto> ApproveAsync(int id);
}
