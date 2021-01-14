using System;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public partial class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid user name.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a valid password.")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string AccessToken { get; set; }
        public DateTime expiry { get; set; }
        public Role Role { get; set; }
    }
}

public enum Role
{
    Admin = 1,
    Customer = 2
}