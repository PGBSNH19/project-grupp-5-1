using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 500, ErrorMessage = "Stock must be between 1 and 500 .")]
        public int Stock { get; set; }
        [Required]
        [StringLength(16, ErrorMessage = "Name too long (16 character limit).")]
        public string Name { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Description too long (250 character limit).")]
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public int ProductCategoryId { get; set; }
    }
}