using Backend.Models;
using System.Collections.Generic;

namespace Backend.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int Stock { get; set; }
        public int ProductCategoryId { get; set; }
        public ICollection<ProductPrice> ProductPrices { get; set; }
        public string DefaultImageName { get; set; }
    }
}