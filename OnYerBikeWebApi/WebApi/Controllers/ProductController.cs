using DAL.Repositories.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DjBikeShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]   
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IGenericRepository<Product> _repository;
        private readonly IProductRepository _productRepository;

        public ProductController(ILogger<ProductController> logger,
            IGenericRepository<Product> repository, 
            IProductRepository productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            try
            {              
                return Ok(_repository.GetAll()
                    .Where(p => p.ListPrice > 0));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProducts failed: {ex}");
                return BadRequest("GetAllProducts failed");
            }
        }      

        [HttpGet("{productName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> GetProductName(string productName)
        {
            try
            {
                return Ok(_productRepository.GetProductsByName(productName));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductName failed: {ex}");
                return BadRequest("GetProductName failed");
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Product model not valid");
                    return BadRequest("Product model not valid");
                }

                product.ProductId = null;

                _repository.Insert(product);
                _repository.Save();

                return Created($"/api/product/{product.ProductId}", product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProduct failed: {ex}");
                return BadRequest("CreateProduct failed");
            }
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Product model not valid");
                    return BadRequest("Product model not valid");
                }
               
                _repository.Update(product);
                _repository.Save();

                return Ok("Product successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProduct failed: {ex}");
                return BadRequest("UpdateProduct failed");
            }
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                _repository.Delete(productId);
                _repository.Save();

                return Ok("Product successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProduct failed: {ex}");
                return BadRequest("DeleteProduct failed");
            }
        }

    }
}
