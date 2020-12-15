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
    [Route("api/v1.0/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ILogger<CouponController> _logger;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponController(ILogger<CouponController> logger, ICouponRepository couponRepository, IMapper mapper)
        {
            _logger = logger;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCoupons()
        {
            try
            {
                return Ok(await _couponRepository.GetAll());
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: { e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }

        public async Task<ActionResult<Coupon>> GetCouponById(int id)
        {
            try
            {
                return Ok(await _couponRepository.GetCouponById(id));
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: { e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }

        public async Task<ActionResult<Coupon>> CreateNewCoupon(Coupon coupon)
        {
            try
            {
                var mappedEntity = _mapper.Map<Coupon>(coupon);
                await _couponRepository.Add(mappedEntity);
                if(await _couponRepository.Save())
                {
                    return Created($"api/v1.0/coupon/{mappedEntity.Id}", _mapper.Map<Coupon>(mappedEntity));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
            return BadRequest();
        }
    }
}



    //public WeatherController(ILogger<WeatherController> logger, IWeatherRepository weatherRepository, IMapper mapper)
    //{
    //    _logger = logger;
    //    _weatherRepository = weatherRepository;
    //    _mapper = mapper;
    //}
}
