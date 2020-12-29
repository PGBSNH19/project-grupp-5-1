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
using Backend.Models.Mail;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class OrderedProductsController : ControllerBase
    {
        private readonly ILogger<OrderedProductsController> _logger;
        private readonly IOrderedProductRepository _orderProductRepository;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public OrderedProductsController(ILogger<OrderedProductsController> logger, IOrderedProductRepository orderProductRepository, IMailService mailService, IMapper mapper)
        {
            _logger = logger;
            _orderProductRepository = orderProductRepository;
            _mailService = mailService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all ordered products.
        /// </summary>
        /// <returns>A list of all ordered products.</returns>
        /// <response code="200">Returns a list of all ordered products.</response>
        /// <response code="404">There are no ordered products in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch the ordered products.</response>   
        [HttpGet]
        public async Task<ActionResult<OrderedProductDTO[]>> GetAll()
        {
            try
            {
                var orderProducts = await _orderProductRepository.GetAll();

                if (orderProducts == null)
                {
                    return NotFound($"Could not find any orderProduct");
                }

                _logger.LogInformation($"Fetching all orderProduct from the database.");
                var mappedResults = _mapper.Map<IList<OrderedProductDTO>>(orderProducts);
                return Ok(mappedResults);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves an ordered product by its Id.
        /// </summary>
        /// <param name="orderProductId">The Id of the ordered product.</param>
        /// <returns>The ordered product which has the specified Id.</returns>
        /// <response code="200">Returns the ordered product which matched the given Id.</response>
        /// <response code="404">No ordered product was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch an ordered product.</response>
        [HttpGet("{orderProductId}")]
        public async Task<ActionResult<OrderedProductDTO>> GetById(int orderProductId)
        {
            try
            {
                var orderProduct = await _orderProductRepository.Get(orderProductId);

                if (orderProduct == null)
                {
                    return NotFound($"OrderProduct with id {orderProductId} was not found.");
                }

                _logger.LogInformation($"Fetching the orderProduct of Id number: {orderProductId} from the database.");
                var mappedResult = _mapper.Map<OrderedProductDTO>(orderProduct);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new ordered product.
        /// </summary>
        /// <param name="orderProduct">The new ordered product object to be added.</param>
        /// <returns>The ordered product object which has been added.</returns>
        /// <response code="200">Returns the new ordered product which has been added.</response>
        /// <response code="400">The API failed to save the new ordered product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an ordered product.</response> 
        [HttpPost]
        public async Task<ActionResult<OrderedProductDTO>> Add([FromBody] OrderedProductDTO orderProduct)
        {
            try
            {
                var mappedResult = _mapper.Map<OrderedProduct>(orderProduct);
                await _orderProductRepository.Add(mappedResult);

                if (await _orderProductRepository.Save())
                {
                    _logger.LogInformation($"Inserting an new orderProduct to the database.");
                    return Ok(_mapper.Map<OrderedProductDTO>(mappedResult));
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
        /// Updates an existing ordered product.
        /// </summary>
        /// <param name="orderProductId">The Id of the requested ordered product which will be updated.</param>
        /// <param name="updatedOrderProduct">The new details of the ordered product object.</param>
        /// <returns>The ordered product object with its updated details.</returns>
        /// <response code="200">Returns the ordered product which has been updated.</response>
        /// <response code="404">No ordered product was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the ordered product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an ordered product.</response>  
        [HttpPut("{orderProductId}")]
        public async Task<ActionResult<OrderedProduct>> Update(int orderProductId, [FromBody] OrderedProductDTO updatedOrderProduct)
        {
            try
            {
                var orderProduct = await _orderProductRepository.Get(orderProductId);

                if (orderProduct == null)
                {
                    return NotFound($"OrderProduct with id {orderProductId} was not found.");
                }

                var mappedResult = _mapper.Map(updatedOrderProduct, orderProduct);
                mappedResult.Id = orderProductId;
                _orderProductRepository.Update(mappedResult);

                if (await _orderProductRepository.Save())
                {
                    _logger.LogInformation($"Updating the orderProduct of Id number: {orderProductId} in the database.");
                    return Ok(_mapper.Map<OrderedProductDTO>(mappedResult));
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
        /// Deletes an existing ordered product.
        /// </summary>
        /// <param name="orderProductId">The Id of the ordered product which needs to be deleted.</param>
        /// <returns>The deleted ordered product object.</returns>
        /// <response code="200">Returns the ordered product which has been deleted.</response>
        /// <response code="404">No ordered product was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the ordered product.</response>
        /// <response code="500">The API caught an exception when attempting to delete an ordered product.</response>  
        [HttpDelete("{orderProductId}")]
        public async Task<ActionResult<OrderedProductDTO>> Delete(int orderProductId)
        {
            try
            {
                var orderProduct = await _orderProductRepository.Get(orderProductId);

                if (orderProduct == null)
                {
                    return NotFound($"OrderProduct with id {orderProductId} was not found.");
                }

                _orderProductRepository.Remove(orderProduct);

                if (await _orderProductRepository.Save())
                {
                    _logger.LogInformation($"Deleting the orderProduct of Id number: {orderProductId} from the database.");
                    return Ok(_mapper.Map<OrderedProductDTO>(orderProduct));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the order. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendMail([FromBody] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok(request);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}