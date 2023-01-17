using Data.Models;
using WebApi.Models;

namespace Data.Repositories.Abstract
{
	public interface IOrderRepository
	{
		List<OrderDetailDto> GetOrderDetails(int orderId);

        void CreateOrder(OrderDto order);

	}
}
