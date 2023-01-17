using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
	public class OrderLine
	{
		[Key]
		public int? OrderLineId { get; set; }

		public int? OrderId { get; set; }
		public int ProductId { get; set; }
		public int? Quantity { get; set; }

	}
}
