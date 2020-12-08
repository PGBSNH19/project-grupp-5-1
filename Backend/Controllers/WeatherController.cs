using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public WeatherController(ILogger<WeatherController> logger, IWeatherRepository weatherRepository)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
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

    }
}
