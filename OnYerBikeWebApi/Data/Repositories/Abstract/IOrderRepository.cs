

using Data.Dtos;

namespace Data.Repositories.Abstract
{
	public interface IOrderRepository
	{
		List<OrderDetailDto> GetOrderDetails(int orderId);

        void CreateOrder(OrderDto order);

	}
}
