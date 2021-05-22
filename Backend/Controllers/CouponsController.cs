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

        /// <summary>
        /// Retrieves all existing coupons.
        /// </summary>
        /// <returns>A list of all existing coupons.</returns>
        /// <response code="200">Returns a list of existing coupons.</response>
        /// <response code="404">There are no coupons stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch coupons.</response>    
        [HttpGet]
        public async Task<ActionResult<CouponDTO[]>> GetAll()
        {
            try
            {
                var coupons = await _couponRepository.GetAll();

                if (coupons == null)
                {
                    return NotFound($"Could not find any coupons.");
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

        /// <summary>
        /// Retrieves a coupon by its Id.
        /// </summary>
        /// <param name="couponId">The Id of the requested coupon.</param>
        /// <returns>The coupon which has the specified Id.</returns>
        /// <response code="200">Returns the coupon which matched the given Id.</response>
        /// <response code="404">No coupon was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch a coupon.</response>    
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

        /// <summary>
        /// Adds a new coupon.
        /// </summary>
        /// <param name="coupon">The new coupon object to be added.</param>
        /// <returns>The coupon object which has been added.</returns>
        /// <response code="200">Returns the new coupon which has been added.</response>
        /// <response code="400">The API failed to save the new coupon to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a coupon.</response>    
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

        /// <summary>
        /// Updates an existing coupon.
        /// </summary>
        /// <param name="couponId">The Id of the requested coupon which will be updated.</param>
        /// <param name="updatedCoupon">The new details of the coupon object.</param>
        /// <returns>The coupon object with its updated details.</returns>
        /// <response code="200">Returns the coupon which has been updated.</response>
        /// <response code="404">No coupon was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated coupon to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a coupon.</response>  
        [HttpPut("{couponId}")]
        public async Task<ActionResult<CouponDTO>> Update(int couponId, [FromBody] CouponDTO updatedCoupon)
        {
            try
            {
                var coupon = await _couponRepository.Get(couponId);

                if (coupon == null)
                {
                    return NotFound($"Coupon with id {couponId} was not found.");
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

        /// <summary>
        /// Deletes an existing coupon.
        /// </summary>
        /// <param name="couponId">The Id of the coupon which needs to be deleted.</param>
        /// <returns>The deleted coupon object.</returns>
        /// <response code="200">Returns the coupon which has been deleted.</response>
        /// <response code="404">No coupon was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the coupon.</response>
        /// <response code="500">The API caught an exception when attempting to delete a coupon.</response>  
        [HttpDelete("{couponId}")]
        public async Task<ActionResult<CouponDTO>> Delete(int couponId)
        {
            try
            {
                var coupon = await _couponRepository.Get(couponId);

                if (coupon == null)
                {
                    return NotFound($"Coupon with id {couponId} was not found.");
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
    }
}