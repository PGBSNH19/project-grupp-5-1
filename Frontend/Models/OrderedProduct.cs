﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
