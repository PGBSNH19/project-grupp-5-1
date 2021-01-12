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
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IConfiguration config, IUserRepository userRepository, IMapper mapper, ILogger<LoginController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user in the system.
        /// </summary>
        /// <param name="user">The user details which will be used to login.</param>
        /// <returns>The login details of the authenticated user.</returns>
        /// <response code="200">Returns the login details of the authenticated user.</response>
        /// <response code="401">The given login details were wrong.</response>   
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

        /// <summary>
        /// Register a new user in the system.
        /// </summary>
        /// <param name="user">The user details which will be used to register.</param>
        /// <returns>The login details of the authenticated user.</returns>
        /// <response code="200">Returns the registered and login details of the new user.</response>
        /// <response code="404">There are no users stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch users.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO user)
        {
            try
            {
                var mappedResult = _mapper.Map<User>(user);

                var users = await _userRepository.GetAll();

                if (users.Where(u => u.Username == user.Username).FirstOrDefault() == null)
                {
                    mappedResult.Password = HashPassword(mappedResult.Password);

                    await _userRepository.Add(mappedResult);

                    if (await _userRepository.Save())
                    {
                        _logger.LogInformation($"Inserting an new user to the database.");

                        UserDTO authenticationResult = AuthenticateUser(user);

                        var token = GenerateJWTToken(authenticationResult);
                        return Ok(new
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
                }
                else
                {
                    _logger.LogInformation($"This user is already exist in database.");
                    return StatusCode(409, $"User '{user.Username}' already exists.");
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the order. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
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