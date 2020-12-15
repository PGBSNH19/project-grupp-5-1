using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        public int Stock { get; set; }
        [Required]
        public ProductCategory ProductCategory { get; set; }
    }
}
