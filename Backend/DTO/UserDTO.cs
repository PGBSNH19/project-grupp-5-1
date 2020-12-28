using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //[Required]
        //[Range(1, 2, ErrorMessage = "You need to write a number betweem 1 and 2")]
        public int Role { get; set; }
    }
}
