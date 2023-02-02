using Data.Dtos;

namespace WebApi.Services.Abstract
{
    public interface IAuthManager
    {

        Task<bool> ValidateUser(LoginUserDto loginUserDto);
        Task<string> CreateToken();

    }
}
