using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ProductCategory
    {

        [Key]
        public int? ProductCategoryId { get; set; }
        public string? Name { get; set; }

    }
}
