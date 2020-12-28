namespace Frontend.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccessToken { get; set; }
        public Role Role { get; set; }
    }
}

public enum Role
{
    Admin = 1,
    Customer = 2
}