using DAL.Context;
using Data.Models;
using Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public void CreateOrder(OrderHeader order)
		{
			_context.Orders.Add(order);
			_context.SaveChanges();

			foreach (var orderLine in order.OrderLines)
			{
				_context.OrderLines.Add(new OrderLine
				{
					OrderHeaderId = order.OrderHeaderId,
					ProductId = orderLine.ProductId
				});
			}

			_context.SaveChanges();

		}

	}
}
