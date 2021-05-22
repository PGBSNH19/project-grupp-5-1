using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTO
{
    public class OrderedProductDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
