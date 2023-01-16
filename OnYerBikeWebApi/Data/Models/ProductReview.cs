using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class ProductReview
    {

        [Key]
        public int? ProductReviewId { get; set; }
        public int? ProductId { get; set; }
        public string? ReviewerName { get; set; }
        public string? ReviewDate { get; set; }
        public string? EmailAddress { get; set; }
        public string? Rating { get; set; }
        public string? Comments { get; set; }


    }
}
