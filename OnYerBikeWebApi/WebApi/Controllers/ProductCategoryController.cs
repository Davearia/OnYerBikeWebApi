using DAL.Models;
using DAL.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DjBikeShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {

        private readonly ILogger<ProductCategoryController> _logger;
        private readonly IGenericRepository<ProductCategory> _repository;

        public ProductCategoryController(ILogger<ProductCategoryController> logger,
            IGenericRepository<ProductCategory> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));            
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<ProductCategory>> GetAllProductCategories()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProductCategories failed: {ex}");
                return BadRequest("GetAllProductCategories failed");
            }
        }

        [HttpGet("{productCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ProductCategory> GetProductCategoryById(int productCategoryId)
        {
            try
            {
                return Ok(_repository.GetById(productCategoryId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductCategoryById failed: {ex}");
                return BadRequest("GetProductCategoryById failed");
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProductCategory([FromBody] ProductCategory productCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ProductCategory model not valid");
                    return BadRequest("ProductCategory model not valid");
                }

                productCategory.ProductCategoryId = null;

                _repository.Insert(productCategory);
                _repository.Save();

                return Created($"/api/productCategory/{productCategory.ProductCategoryId}", productCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProductCategory failed: {ex}");
                return BadRequest("CreateProductCategory failed");
            }
        }

        [HttpPut("{productCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProductCategory(int productCategoryId, [FromBody] ProductCategory productCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ProductCategory model not valid");
                    return BadRequest("ProductCategory model not valid");
                }

                productCategory.ProductCategoryId = productCategoryId;
                _repository.Update(productCategory);
                _repository.Save();

                return Ok("ProductCategory successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProductCategory failed: {ex}");
                return BadRequest("UpdateProductCategory failed");
            }
        }

        [HttpDelete("{productCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProductCategory(int productCategoryId)
        {
            try
            {
                _repository.Delete(productCategoryId);
                _repository.Save();

                return Ok("ProductCategory successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProductCategory failed: {ex}");
                return BadRequest("DeleteProductCategory failed");
            }
        }

    }
}
