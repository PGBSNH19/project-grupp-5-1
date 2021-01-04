using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = "Stock must be between 1 and 500 .")]
        public int Stock { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Name too long (60 character limit).")]
        public string Name { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Description too long (250 character limit).")]
        public string Description { get; set; }

        [Required]
        public bool IsFeatured { get; set; }

        public bool IsAvailable { get; set; }

        public int ProductCategoryId { get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }

        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
    }
}