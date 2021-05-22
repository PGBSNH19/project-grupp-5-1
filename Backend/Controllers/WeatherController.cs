using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DTO;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IMapper _mapper;

        public WeatherController(ILogger<WeatherController> logger, IWeatherRepository weatherRepository, IMapper mapper)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Weather[]>> GetAll()
        {
            try
            {
                var results = await _weatherRepository.GetAll();

                if (results == null)
                {
                    return NotFound($"Could not find any weather");
                }
                return Ok(results);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        [HttpGet("{weatherId}")]
        public async Task<ActionResult<Weather>> GetById(int weatherId)
        {
            try
            {
                var weather = await _weatherRepository.GetWeatherById(weatherId);

                if (weather == null)
                {
                    return NotFound($"Weather with id {weatherId} was not found.");
                }

                return Ok(weather);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to get the weather. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Weather>> Add([FromBody] Weather weather)
        {
            try
            {
                await _weatherRepository.Add(weather);
                if (await _weatherRepository.Save())
                {
                    return Ok(weather);
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the weather. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        [HttpDelete("{weatherId}")]
        public async Task<ActionResult<Weather>> Delete(int weatherId)
        {
            try
            {
                var weather = await _weatherRepository.GetWeatherById(weatherId);

                if (weather == null)
                {
                    return BadRequest($"Weather with id {weatherId} was not found.");
                }

                _weatherRepository.Remove(weather);

                if (await _weatherRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the weather. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{weatherId}")]
        public async Task<ActionResult<Weather>> Update(int weatherId, [FromBody] WeatherToUpdateDto weatherToUpdateDto)
        {
            try
            {
                var weather = await _weatherRepository.GetWeatherById(weatherId);

                if (weather == null)
                {
                    return BadRequest($"Weather with id {weatherId} was not found.");
                }

                _mapper.Map(weatherToUpdateDto, weather);

                if (await _weatherRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to update the weather. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

    }
}
