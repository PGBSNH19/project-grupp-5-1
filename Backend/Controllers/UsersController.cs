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

        // GET: api/v1.0/Users
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

        // GET: api/v1.0/Users/5
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

        // POST: api/v1.0/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Add([FromBody] UserDTO user)
        {
            try
            {
                var mappedResult = _mapper.Map<User>(user);

                await _userRepository.Add(mappedResult);

                if (await _userRepository.Save())
                {
                    _logger.LogInformation($"Inserting an new user to the database.");
                    return Ok(_mapper.Map<UserDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the order. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        // PUT: api/v1.0/Users/5
        [HttpPut("{userId}")]
        public async Task<ActionResult<User>> Update(int userId, [FromBody] UserDTO updatedUser)
        {
            try
            {
                var user = await _userRepository.Get(userId);

                if (user == null)
                {
                    return BadRequest($"User with id {userId} was not found.");
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

        // DELETE: api/v1.0/Users/5
        [HttpDelete("{userId}")]
        public async Task<ActionResult<UserDTO>> Delete(int userId)
        {
            try
            {

                var user = await _userRepository.Get(userId);

                if (user == null)
                {
                    return BadRequest($"User with id {userId} was not found.");
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
    }
}
