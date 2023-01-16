using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Product
    {

        [Key]
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ProductSubcategoryId { get; set; }
        public string? ProductNumber { get; set; }
        public decimal? ListPrice { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public string? ThumbNailPhoto { get; set; }
        public string? ThumbnailPhotoFileName { get; set; }
        public string? LargePhoto { get; set; }
        public string? LargePhotoFileName { get; set; }

    }
}
