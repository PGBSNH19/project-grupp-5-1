using System;
using System.Collections.Generic;
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
    public class CouponsController : ControllerBase
    {
        private readonly ILogger<CouponsController> _logger;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponsController(ILogger<CouponsController> logger, ICouponRepository couponRepository, IMapper mapper)
        {
            _logger = logger;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        // GET: api/v1.0/coupons
        [HttpGet]
        public async Task<ActionResult<CouponDTO[]>> GetAll()
        {
            try
            {
                var coupons = await _couponRepository.GetAll();

                if (coupons == null)
                {
                    return NotFound($"Could not find any coupon");
                }

                _logger.LogInformation($"Fetching all coupons from the database.");
                var mappedResults = _mapper.Map<IList<CouponDTO>>(coupons);
                return Ok(mappedResults);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        // GET: api/v1.0/coupons/5
        [HttpGet("{couponId}")]
        public async Task<ActionResult<CouponDTO>> GetById(int couponId)
        {
            try
            {
                var coupon = await _couponRepository.Get(couponId);

                if (coupon == null)
                {
                    return NotFound($"Coupon with id {couponId} was not found.");
                }

                _logger.LogInformation($"Fetching the coupon of Id number: {couponId} from the database.");
                var mappedResult = _mapper.Map<CouponDTO>(coupon);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        // POST: api/v1.0/coupons
        [HttpPost]
        public async Task<ActionResult<Coupon>> Add(Coupon coupon)
        {
            try
            {
                //var mappedResult = _mapper.Map<Coupon>(coupon);
                await _couponRepository.Add(coupon);

                try
                {
                    await _couponRepository.Save();
                    return Ok(coupon);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the coupon. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

        // PUT: api/v1.0/coupons/5
        [HttpPut("{couponId}")]
        public async Task<ActionResult<CouponDTO>> Update(int couponId, [FromBody] CouponDTO updatedCoupon)
        {
            try
            {
                var coupon = await _couponRepository.Get(couponId);

                if (coupon == null)
                {
                    return BadRequest($"Coupon with id {couponId} was not found.");
                }


                var mappedResult = _mapper.Map(updatedCoupon, coupon);
                mappedResult.Id = couponId;
                _couponRepository.Update(mappedResult);

                if (await _couponRepository.Save())
                {
                    _logger.LogInformation($"Updating the coupon of Id number: {couponId} in the database.");
                    return Ok(_mapper.Map<CouponDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to update the coupon. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        // DELETE: api/v1.0/coupons/5
        [HttpDelete("{couponId}")]
        public async Task<ActionResult<CouponDTO>> Delete(int couponId)
        {
            try
            {

                var coupon = await _couponRepository.Get(couponId);

                if (coupon == null)
                {
                    return BadRequest($"Coupon with id {couponId} was not found.");
                }

                _couponRepository.Remove(coupon);

                if (await _couponRepository.Save())
                {
                    _logger.LogInformation($"Deleting the coupon of Id number: {couponId} from the database.");
                    return Ok(_mapper.Map<CouponDTO>(coupon));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the coupon. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }
        //    [HttpGet]
        //    public async Task<ActionResult> GetAllCoupons()
        //    {
        //        try
        //        {
        //            return Ok(await _couponRepository.GetAll());
        //        }
        //        catch (TimeoutException e)
        //        {
        //            return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: { e.Message}");
        //        }
        //        catch (Exception e)
        //        {
        //            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
        //        }
        //    }

        //    public async Task<ActionResult<Coupon>> GetCouponById(int id)
        //    {
        //        try
        //        {
        //            return Ok(await _couponRepository.GetCouponById(id));
        //        }
        //        catch (TimeoutException e)
        //        {
        //            return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: { e.Message}");
        //        }
        //        catch (Exception e)
        //        {
        //            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
        //        }
        //    }

        //    public async Task<ActionResult<CouponDTO>> CreateNewCoupon(CouponDTO coupon)
        //    {
        //        try
        //        {
        //            var mappedEntity = _mapper.Map<Coupon>(coupon);
        //            await _couponRepository.Add(mappedEntity);

        //            if(await _couponRepository.Save())
        //            {
        //                return Created($"api/v1.0/coupons/{mappedEntity.Id}", _mapper.Map<Coupon>(mappedEntity));
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
        //        }
        //        return BadRequest();
        //    }
    }
}