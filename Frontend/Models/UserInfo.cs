using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class UserInfo
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(100, ErrorMessage = "First name too long (100 character limit).")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(100, ErrorMessage = "Last name too long (100 character limit).")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your delivery address.")]
        [StringLength(100, ErrorMessage = "Address too long (100 character limit).")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your city.")]
        [StringLength(100, ErrorMessage = "City too long (100 character limit).")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter your zip code.")]
        [Range(10000, 99999, ErrorMessage = "Zip Code too long (5 character limit).")]
        public int ZipCode { get; set; }

        public decimal TotalPiceWithDiscount { get; set; } = 0;

        public string Date { get; set; }

        public IEnumerable<ProductInBasket> userBasket { get; set; }
    }
}