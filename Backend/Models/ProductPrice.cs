using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ProductPrice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal SalePrice { get; set; }
        [Required]
        public DateTime DateChanged { get; set; }
        [Required]
        public Product Product { get; set; }
    }
}
