using DAL.Repositories.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DjBikeShopWebAPI.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IGenericRepository<User> _repository;

        public UserController(ILogger<UserController> logger,
            IGenericRepository<User> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<User>> GetAllUseas()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllUseas failed: {ex}");
                return BadRequest("GetAllUseas failed");
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<User>> GetUserById(int userId)
        {
            try
            {
                return Ok(_repository.GetById(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUserById failed: {ex}");
                return BadRequest("GetUserById failed");
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("User model not valid");
                    return BadRequest("User model not valid");
                }

                user.UserId = null;

                _repository.Insert(user);
                _repository.Save();

                return Created($"/api/user/{user.UserId}", user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProduct failed: {ex}");
                return BadRequest("CreateProduct failed");
            }
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProduct(int userId, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("User model not valid");
                    return BadRequest("User model not valid");
                }

                user.UserId = userId;
                _repository.Update(user);
                _repository.Save();

                return Ok("User successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProduct failed: {ex}");
                return BadRequest("UpdateProduct failed");
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProduct(int userId)
        {
            try
            {
                _repository.Delete(userId);
                _repository.Save();

                return Ok("User successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProduct failed: {ex}");
                return BadRequest("DeleteProduct failed");
            }
        }

    }
}
