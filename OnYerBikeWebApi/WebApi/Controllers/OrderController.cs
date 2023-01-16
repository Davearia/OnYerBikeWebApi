using DAL.Repositories.Abstract;
using DAL.Repositories.Concrete;
using Data.Models;
using Data.Repositories.Abstract;
using DjBikeShopWebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{

		private readonly ILogger<OrderController> _logger;		
		private readonly IOrderRepository _orderRepository;

		public OrderController(ILogger<OrderController> logger,
			IOrderRepository orderRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public IActionResult CreateOrder([FromBody] OrderHeader order)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					_logger.LogError("Order model not valid");
					return BadRequest("Order model not valid");
				}

				order.OrderHeaderId = null;
				_orderRepository.CreateOrder(order);

				return Created($"/api/order/{order.OrderHeaderId}", order);
			}
			catch (Exception ex)
			{
				_logger.LogError($"CreateOrder failed: {ex}");
				return BadRequest("CreateOrder failed");
			}
		}

	}
}
