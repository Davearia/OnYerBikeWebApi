using Data.Dtos;

namespace WebApi.Services
{
    public interface IAuthManager
    {

        Task<bool> ValidateUser(UserDto loginUserDto);
        Task<string> CreateToken();

    }
}
