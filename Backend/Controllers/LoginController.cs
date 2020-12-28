using System;
using AutoMapper;
using System.Linq;
using Backend.DTO;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Backend.Services.Interfaces;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public LoginController(IConfiguration config, IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserDTO user)
        {
            IActionResult response = Unauthorized(new { message = "Username or password is incorrect" });
            UserDTO authenticationResult = AuthenticateUser(user);
            if (authenticationResult != null)
            {
                
                var token = GenerateJWTToken(authenticationResult);
                response = Ok(new
                {
                    id = authenticationResult.Id,
                    firstName = authenticationResult.FirstName,
                    lastName = authenticationResult.LastName,
                    username = authenticationResult.Username,
                    role = authenticationResult.Role,
                    accesstoken = new JwtSecurityTokenHandler().WriteToken(token.TokenBody),
                    expiry = token.ExpiryDate,
                });
            }
            return response;
        }

        [NonAction]
        private IEnumerable<UserDTO> Users()
        {
            var allUsers = _userRepository.GetAll().Result.ToList();
            var mappedResult = _mapper.Map<List<UserDTO>>(allUsers);
            return mappedResult;
        }

        [NonAction]
        private UserDTO AuthenticateUser(UserDTO loginCredentials)
        {
            loginCredentials.Password = HashPassword(loginCredentials.Password);
            var user = Users().FirstOrDefault(x => x.Username.ToLower() == loginCredentials.Username.ToLower() && x.Password == loginCredentials.Password);
            if (user != null)
            {
                user.Password = null;
            }
            return user;
        }

        [NonAction]
        private Token GenerateJWTToken(UserDTO userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddMinutes(120);
            Token returnedToken = new Token();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expire,
                signingCredentials: credentials
            );

            returnedToken.ExpiryDate = expire;
            returnedToken.TokenBody = token;

            return returnedToken;
        }

        [NonAction]
        private static string HashPassword(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

    }
}