using DAL.Repositories.Abstract;
using Data.Dtos;
using Data.Entities;
using Data.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{

		private readonly ILogger<OrderController> _logger;
        private readonly IGenericRepository<Order> _repository;
        private readonly IOrderRepository _orderRepository;

		public OrderController(ILogger<OrderController> logger,
             IGenericRepository<Order> repository,
            IOrderRepository orderRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
		}

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return Ok(_orderRepository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrders failed: {ex}");
                return BadRequest("GetAllOrders failed");
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<OrderDetailDto>> GetOrderDetailsById(int orderId)
        {
            try
            {
                return Ok(_orderRepository.GetOrderDetails(orderId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrderDetailsById failed: {ex}");
                return BadRequest("GetOrderDetailsById failed");
            }
        }

        [HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public IActionResult CreateOrder([FromBody] OrderDto order)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					_logger.LogError("Order model not valid");
					return BadRequest("Order model not valid");
				}
				
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(order, options);

				_orderRepository.CreateOrder(order);

				return Created($"/api/order/{order.OrderId}", order);
			}
			catch (Exception ex)
			{
				_logger.LogError($"CreateOrder failed: {ex}");
				return BadRequest("CreateOrder failed");
			}
		}

	}
}
