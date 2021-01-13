using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public partial class RegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid user name.")]
        [StringLength(15, ErrorMessage = "Username is too long (15 character limit).")]
        [MinLength(3, ErrorMessage = "Username must contain a minimum of three characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid password.")]
        [StringLength(15, ErrorMessage = "Password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "Password must be minimum six characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please repeat your password.")]
        [StringLength(15, ErrorMessage = "Your password is too long (15 character limit).")]
        [MinLength(6, ErrorMessage = "Your password must be minimum six characters.")]
        [Compare(nameof(Password), ErrorMessage = "The passwords does not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}