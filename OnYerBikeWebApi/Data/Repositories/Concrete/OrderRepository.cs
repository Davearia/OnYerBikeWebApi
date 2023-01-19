using DAL.Context;
using Data.Dtos;
using Data.Entities;
using Data.Repositories.Abstract;

namespace Data.Repositories.Concrete
{
	public class OrderRepository : IOrderRepository
	{

		private BikeShopDbContext _context;

		public OrderRepository()
		{
			_context = new BikeShopDbContext();
		}

		public OrderRepository(BikeShopDbContext _context)
		{
			this._context = _context;
		}

        public List<OrderDto> GetAllOrders()
        {
			var query = from orders in _context.Orders
				select new OrderDto
				{
					Address = orders.Address,
					City = orders.City,
					Country = orders.Country,
					Name = orders.Name,
					OrderDate = orders.OrderDate,
					OrderId = orders.OrderId,
					PostCode = orders.PostCode,
					Shipped = orders.Shipped != null ? orders.Shipped : false,
					State = orders.State,
					OrderPrice = orders.Cart.CartPrice,
					OrderQuantity = orders.Cart.ItemCount					
				};

            return query.ToList();
        }

        public List<OrderDetailDto> GetOrderDetails(int orderId)
		{
			var query = from products in _context.Products
						join orderLines in _context.OrderLines on products.ProductId equals orderLines.ProductId
						where(orderLines.OrderId == orderId)
						select new OrderDetailDto
						{
							ProductId = orderLines.ProductId,
							Name = products.Name,
							Price = orderLines.Quantity * products.ListPrice,
							Quantity = orderLines.Quantity
						};

			return query.ToList();
        }

		public void CreateOrder(OrderDto orderDto)
		{
			var order = new Order()
			{
				Name = orderDto.Name,
				Address= orderDto.Address,
				City= orderDto.City,
				State= orderDto.State,
				PostCode= orderDto.PostCode,
				Country= orderDto.Country,
				OrderDate = DateTime.Now,
				Cart = new Cart
				{
					CartPrice = orderDto.Cart.CartPrice,
					ItemCount = orderDto.Cart.ItemCount				
				}
			};

			_context.Orders.Add(order);

			_context.SaveChanges();

			foreach (var cartline in orderDto.Cart.Lines)
			{
				_context.OrderLines.Add(new OrderLine
				{
					OrderId = order.OrderId,
					ProductId = (int)cartline.Product.ProductId,
					Quantity = cartline.Quantity
				});
			}

			_context.SaveChanges();

		}

	}
}
