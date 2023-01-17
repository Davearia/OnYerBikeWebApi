using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class ProductSubcategory
    {

        [Key]
        public int? ProductSubCategoryId { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? Name { get; set; }

    }
}
