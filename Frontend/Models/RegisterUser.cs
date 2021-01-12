using System;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public partial class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage ="please enter a valid user name")]
        [StringLength(15, ErrorMessage = "user name is too long (15 character limit).")]
        [MinLength(3, ErrorMessage = "user name must be minimum three characters..")]
        public string Username { get; set; }

        [Required(ErrorMessage = "please enter a valid password")]
        [StringLength(15, ErrorMessage = "password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "password must be minimum six characters..")]
        public string Password { get; set; }

        [Required(ErrorMessage = "please repeat the password")]
        [StringLength(15, ErrorMessage = "password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "password must be minimum six characters..")]
        [Compare(nameof(Password), ErrorMessage ="passwords are not matched..")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}
