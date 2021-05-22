using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public bool IsDefault { get; set; } = false;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
