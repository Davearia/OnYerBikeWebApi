using Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Cart
    {
        public Cart()
        {
            Lines = new List<CartLineDto>();          
        }

        [Key]
        public int? CartId { get; set; }
        public int? ItemCount { get; set; }
        public decimal? CartPrice { get; set; }
        public List<CartLineDto> Lines;        

    }
}
