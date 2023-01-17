using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Data.Dtos
{
    public class CartDto
    {
        public CartDto()
        {
            Lines = new List<CartLineDto>();          
        }
   
        public int? Id { get; set; }
        public int? ItemCount { get; set; }
        public decimal? CartPrice { get; set; }
        public List<CartLineDto> Lines { get; set; }        

    }
}
