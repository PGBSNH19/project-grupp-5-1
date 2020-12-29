using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.Mail
{
    public class BuyedProducts
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Aomunt { get; set; }
        public string Price { get; set; }
        public int TotalPrice { get; set; }
    }
}