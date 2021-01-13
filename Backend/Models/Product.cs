using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public bool IsFeatured { get; set; }

        [Required]
        public int Stock { get; set; }

        [ForeignKey("ProductCategory")]
        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<OrderedProduct> OrderedProducts { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}