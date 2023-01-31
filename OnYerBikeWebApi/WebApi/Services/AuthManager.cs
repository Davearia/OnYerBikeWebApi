using Data.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Services
{
    public class AuthManager : IAuthManager
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration.GetSection("Jwt:Key").Value ?? string.Empty;
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            //TODO try 512
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.Email)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
               claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var issuer = _configuration.GetSection("Jwt:Issuer").Value;

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            return token;
        }

        public async Task<bool> ValidateUser(UserDto loginUserDto)
        {
            _user = await _userManager.FindByEmailAsync(loginUserDto.Email);

            return (_user != null && await _userManager.CheckPasswordAsync(_user, loginUserDto.Password));
        }

    }
}
