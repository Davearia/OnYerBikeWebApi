using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class OrderHeader
	{

		public OrderHeader()
		{
			OrderLines = new List<OrderLine>();
		}

		[Key]
		public int? OrderHeaderId { get; set; }
		public string? Name { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? PostCode { get; set; }
		public string? Country { get; set; }
		public decimal TotalPrice { get; set; }

		public List<OrderLine> OrderLines;

	}
}
