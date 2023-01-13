
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class ProductCategory
    {

        [Key]
        public int? ProductCategoryId { get; set; }
        public string? Name { get; set; }

    }
}
