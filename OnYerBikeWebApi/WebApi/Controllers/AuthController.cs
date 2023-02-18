using AutoMapper;
using Data.Dtos;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Abstract;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApiUser> _userManager;
        private readonly IAuthManager _authManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApiUser> userManager,
            IAuthManager authManager,
            ILogger<AuthController> logger,            
            IMapper mapper
            )
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _authManager = authManager ?? throw new ArgumentException(nameof(authManager));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest("Register failed");
                }

                await _userManager.AddToRolesAsync(user, userDto.Roles);

                return Ok($"Register successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in {nameof(Register)}");
                return Problem($"Error occured in {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            _logger.LogInformation($"Login attempt for {userDto.Email}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
				var authResponse = await _authManager.Login(userDto);
				if (authResponse == null)
				{
					return Unauthorized();
				}
				return Ok(authResponse);

				//if (!await _authManager.ValidateUser(userDto))
    //            {
    //                return Unauthorized();
    //            }

    //            userDto.Token = await _authManager.CreateToken();

    //            return Accepted(userDto);                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in {nameof(Login)}");
                return Problem($"Error occured in {nameof(Login)}", statusCode: 500);
            }
        }

    }
}
