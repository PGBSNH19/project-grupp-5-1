using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}