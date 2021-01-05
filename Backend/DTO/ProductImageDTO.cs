using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTO
{
    public class ProductImageDTO
    {
        public int Id { get; set; }

        public string  ImageName { get; set; }

        public int ProductId { get; set; }

        public bool IsDefault { get; set; } = false;
    }
}
