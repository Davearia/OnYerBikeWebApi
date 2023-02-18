using Data.Dtos;
using System.Threading.Tasks;

namespace WebApi.Services.Abstract
{
    public interface IAuthManager
    {

        //Task<bool> ValidateUser(LoginUserDto loginUserDto);
        //Task<string> CreateToken();

        Task<LoginUserDto?> Login(LoginUserDto loginDto);


	}
}
