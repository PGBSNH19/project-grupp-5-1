using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateRegistered { get; set; }
        [Required]
        public Coupon Coupon { get; set; }
        [Required]
        public User User { get; set; }

        public ICollection<OrderedProduct> OrderedProduct { get; set; }
    }
}
