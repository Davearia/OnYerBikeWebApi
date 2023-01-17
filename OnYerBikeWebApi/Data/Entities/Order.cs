using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
	public class Order
	{
				
        [Key]
		public int? OrderId { get; set; }
		public string? Name { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? PostCode { get; set; }
		public string? Country { get; set; }
        public bool? Shipped { get; set; }
        public DateTime? OrderDate { get; set; }
		public Cart? Cart { get; set; }   		
			
	}
}
