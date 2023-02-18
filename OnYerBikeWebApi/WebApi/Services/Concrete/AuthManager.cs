using Data.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Services.Abstract;

namespace WebApi.Services.Concrete
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

        //public async Task<string> CreateToken()
        //{
        //    var signingCredentials = GetSigningCredentials();
        //    var claims = await GetClaims();
        //    var token = GenerateTokenOptions(signingCredentials, claims);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        //private SigningCredentials GetSigningCredentials()
        //{
        //    var key = _configuration.GetSection("Jwt:Key").Value ?? string.Empty;
        //    var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
         
        //    return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        //}

        //private async Task<List<Claim>> GetClaims()
        //{
        //    var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name, _user.Email)
        //    };

        //    var roles = await _userManager.GetRolesAsync(_user);

        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    return claims;
        //}

        //private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        //{
        //    var issuer = _configuration.GetSection("Jwt:Issuer").Value;

        //    var token = new JwtSecurityToken(
        //        issuer: issuer,
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(10),
        //        signingCredentials: signingCredentials);

        //    return token;
        //}

        //public async Task<bool> ValidateUser(LoginUserDto loginUserDto)
        //{
        //    _user = await _userManager.FindByEmailAsync(loginUserDto.Email);

        //    return _user != null && await _userManager.CheckPasswordAsync(_user, loginUserDto.Password);
        //}

		public async Task<LoginUserDto?> Login(LoginUserDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (user == null || isValidUser == false)
			{
				return null;
			}

			loginDto.Token = await GenerateToken(user);

            return loginDto;
		}

		private async Task<string> GenerateToken(ApiUser user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
			var userClaims = await _userManager.GetClaimsAsync(user);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id),
			}
			.Union(userClaims).Union(roleClaims);
			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:ValidAudience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Jwt:DurationInMinutes"])),
				signingCredentials: credentials
				);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}


	}
}
