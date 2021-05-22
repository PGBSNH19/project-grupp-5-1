using System;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public partial class RegisterUser
    {
        [StringLength(15, ErrorMessage = "First name is too long (15 character limit).")]
        [MinLength(3, ErrorMessage = "First name must be minimum three characters..")]
        public string FirstName { get; set; }

        [StringLength(15, ErrorMessage = "Last name is too long (15 character limit).")]
        [MinLength(3, ErrorMessage = "Last name must be minimum three characters..")]
        public string LastName { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "User name is too long (15 character limit).")]
        [MinLength(3, ErrorMessage = "User name must be minimum three characters..")]
        public string Username { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "Password must be minimum six characters..")]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Passwords are not matched..")]
        [StringLength(15, ErrorMessage = "Password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "Password must be minimum six characters..")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}
