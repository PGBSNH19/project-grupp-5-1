using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateRegistered { get; set; }
        public string CouponId { get; set; }
        public int UserId { get; set; }
    }
}
