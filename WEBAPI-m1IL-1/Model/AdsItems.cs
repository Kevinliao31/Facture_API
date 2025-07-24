using System.ComponentModel.DataAnnotations;

namespace WEBAPI_m1IL_1.Models
{
    public class AdsItems
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0, 100000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
