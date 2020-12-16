using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
    }
}