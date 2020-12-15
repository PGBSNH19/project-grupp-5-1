using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class OrderedProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public Product Product { get; set; }
    }
}
