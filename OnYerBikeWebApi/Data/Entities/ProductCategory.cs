using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ProductCategory
    {

        [Key]
        public int? ProductCategoryId { get; set; }
        public string? Name { get; set; }

    }
}
