using System;
using AutoMapper;
using Backend.DTO;
using Backend.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all registered users.
        /// </summary>
        /// <returns>A list of all registered users.</returns>
        /// <response code="200">Returns a list of registered users.</response>
        /// <response code="404">There are no users stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch users.</response>
        [HttpGet]
        public async Task<ActionResult<UserDTO[]>> GetAll()
        {
            try
            {
                var users = await _userRepository.GetAll();

                if (users == null)
                {
                    return NotFound($"Could not find any user");
                }

                _logger.LogInformation($"Fetching all users from the database.");
                var mappedResults = _mapper.Map<IList<UserDTO>>(users);
                return Ok(mappedResults);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves an user by its Id.
        /// </summary>
        /// <param name="userId">The Id of the requested user.</param>
        /// <returns>The user which has the specified Id.</returns>
        /// <response code="200">Returns the user which matched the given Id.</response>
        /// <response code="404">No user was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch an user.</response>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetById(int userId)
        {
            try
            {
                var user = await _userRepository.Get(userId);

                if (user == null)
                {
                    return NotFound($"User with id {userId} was not found.");
                }

                _logger.LogInformation($"Fetching the user of Id number: {userId} from the database.");
                var mappedResult = _mapper.Map<UserDTO>(user);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The new user object to be added.</param>
        /// <returns>The user object which has been added.</returns>
        /// <response code="200">Returns the new user which has been added.</response>
        /// <response code="400">The API failed to save the new user to the database.</response>
        /// <response code="409">The API caught an exception when find the user name is already exist in the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an user.</response>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Add([FromBody] UserDTO user)
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
                        return Ok(_mapper.Map<UserDTO>(mappedResult));
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

        /// <summary>
        /// Updates the user details.
        /// </summary>
        /// <param name="userId">The Id of the requested user which will be updated.</param>
        /// <param name="updatedUser">The new details of the user object.</param>
        /// <returns>The user object with its updated details.</returns>
        /// <response code="200">Returns the user which has been updated.</response>
        /// <response code="404">No user was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated user to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an user.</response>  
        [HttpPut("{userId}")]
        public async Task<ActionResult<User>> Update(int userId, [FromBody] UserDTO updatedUser)
        {
            try
            {
                var user = await _userRepository.Get(userId);

                if (user == null)
                {
                    return NotFound($"User with id {userId} was not found.");
                }

                var mappedResult = _mapper.Map(updatedUser, user);
                mappedResult.Id = userId;
                _userRepository.Update(mappedResult);

                if (await _userRepository.Save())
                {
                    _logger.LogInformation($"Updating the user of Id number: {userId} in the database.");
                    return Ok(_mapper.Map<UserDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to update the order. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="userId">The Id of the user which needs to be deleted.</param>
        /// <returns>The deleted user object.</returns>
        /// <response code="200">Returns the user which has been deleted.</response>
        /// <response code="404">No user was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the user.</response>
        /// <response code="500">The API caught an exception when attempting to delete an user.</response>  
        [HttpDelete("{userId}")]
        public async Task<ActionResult<UserDTO>> Delete(int userId)
        {
            try
            {
                var user = await _userRepository.Get(userId);

                if (user == null)
                {
                    return NotFound($"User with id {userId} was not found.");
                }

                _userRepository.Remove(user);

                if (await _userRepository.Save())
                {
                    _logger.LogInformation($"Deleting the user of Id number: {userId} from the database.");
                    return Ok(_mapper.Map<UserDTO>(user));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the order. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        [NonAction]
        private static string HashPassword(string input)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
