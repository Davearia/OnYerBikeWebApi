using Data.Entities;

namespace Data.Dtos
{
    public class CartLineDto
    {

        public Product? Product { get; set; }
        public decimal? lineTotal { get; set; }
        public int? Quantity { get; set; }

    }
}
