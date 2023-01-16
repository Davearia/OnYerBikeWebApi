using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
	public class OrderLine
	{
		[Key]
		public int? OrderLineId { get; set; }

		public int? OrderHeaderId { get; set; }
		public int ProductId { get; set; }

	}
}
