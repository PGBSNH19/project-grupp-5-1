using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserRole Role { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Customer = 2
    }
}
