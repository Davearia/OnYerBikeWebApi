using DAL.Repositories.Abstract;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DjBikeShopWebAPI.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {

        private readonly ILogger<ProductReviewController> _logger;
        private readonly IGenericRepository<ProductReview> _repository;

        public ProductReviewController(ILogger<ProductReviewController> logger,
            IGenericRepository<ProductReview> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<ProductReview>> GetAllProductReviews()
        {
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllProductReviews failed: {ex}");
                return BadRequest("GetAllProductReviews failed");
            }
        }

        [HttpGet("{productReviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ProductReview> GetProductReviewById(int productReviewId)
        {
            try
            {
                return Ok(_repository.GetById(productReviewId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProductReviewById failed: {ex}");
                return BadRequest("GetProductReviewById failed");
            }
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProductReview([FromBody] ProductReview productReview)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ProductReview model not valid");
                    return BadRequest("ProductReview model not valid");
                }

                productReview.ProductReviewId = null;

                _repository.Insert(productReview);
                _repository.Save();

                return Created($"/api/productReview/{productReview.ProductReviewId}", productReview);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProductReview failed: {ex}");
                return BadRequest("CreateProductReview failed");
            }
        }

        [HttpPut("{productReviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProductReview(int productReviewId, [FromBody] ProductReview productReview)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("ProductReview model not valid");
                    return BadRequest("ProductReview model not valid");
                }

                productReview.ProductReviewId = productReviewId;
                _repository.Update(productReview);
                _repository.Save();

                return Ok("ProductCategory successfully updated");
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateProductReview failed: {ex}");
                return BadRequest("UpdateProductReview failed");
            }
        }

        [HttpDelete("{productReviewId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProductReview(int productReviewId)
        {
            try
            {
                _repository.Delete(productReviewId);
                _repository.Save();

                return Ok("ProductReview successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteProductReview failed: {ex}");
                return BadRequest("DeleteProductReview failed");
            }
        }

    }
}
