using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class ProductPrice
    {
       
        public int Id { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Price must be between 0-100000")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalePrice { get; set; }

        [Required]
        public DateTime DateChanged { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

