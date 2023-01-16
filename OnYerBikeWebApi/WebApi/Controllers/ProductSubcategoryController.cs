using DAL.Repositories.Abstract;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace DjBikeShopWebAPI.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProductSubcategoryController : ControllerBase
    {

        private readonly ILogger<ProductSubcategoryController> _logger;
        private readonly IGenericRepository<ProductSubcategory> _repository;

        public ProductSubcategoryController(ILogger<ProductSubcategoryController> logger,
            IGenericRepository<ProductSubcategory> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<ProductSubcategory>> GetAllProductSubCategories()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProductSubCategories failed: {ex}");
                return BadRequest("GetAllProductSubCategories failed");
            }
        }

        [HttpGet("{productSubCategory}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<ProductSubcategory>> GetProductSubCategoryById(int productSubCategory)
        {
            try
            {
                return Ok(_repository.GetById(productSubCategory));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductSubCategoryById failed: {ex}");
                return BadRequest("GetProductSubCategoryById failed");
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProductSubCategory([FromBody] ProductSubcategory productSubCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("productSubCategory model not valid");
                    return BadRequest("ProductSubCategory model not valid");
                }

                productSubCategory.ProductSubCategoryId = null;

                _repository.Insert(productSubCategory);
                _repository.Save();

                return Created($"/api/productSubCategory/{productSubCategory.ProductSubCategoryId}", productSubCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProductSubCategory failed: {ex}");
                return BadRequest("CreateProductSubCategory failed");
            }
        }

        [HttpPut("{productSubCategoryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProductSubCategory(int productSubCategoryId, [FromBody] ProductSubcategory productSubCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ProductSubCategory model not valid");
                    return BadRequest("ProductSubCategory model not valid");
                }

                productSubCategory.ProductSubCategoryId = productSubCategoryId;
                _repository.Update(productSubCategory);
                _repository.Save();

                return Ok("Product successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProductSubCategory failed: {ex}");
                return BadRequest("UpdateProductSubCategory failed");
            }
        }

        [HttpDelete("{productSubCategory}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProductSubCategory(int productSubcategoryId)
        {
            try
            {
                _repository.Delete(productSubcategoryId);
                _repository.Save();

                return Ok("ProductSubCategory successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProductSubCategory failed: {ex}");
                return BadRequest("DeleteProductSubCategory failed");
            }
        }


    }
}
