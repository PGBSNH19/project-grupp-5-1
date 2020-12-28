using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Token
    {
        public DateTime ExpiryDate { get; set; }
        public JwtSecurityToken TokenBody { get; set; }
    }
}
