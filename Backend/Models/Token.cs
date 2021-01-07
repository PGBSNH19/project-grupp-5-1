using System;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Models
{
    public class Token
    {
        public DateTime ExpiryDate { get; set; }
        public JwtSecurityToken TokenBody { get; set; }
    }
}
