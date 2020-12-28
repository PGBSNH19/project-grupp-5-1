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
    public class OrderedProductsController : ControllerBase
    {
        private readonly ILogger<OrderedProductsController> _logger;
        private readonly IOrderedProductRepository _orderProductRepository;
        private readonly IMapper _mapper;

        public OrderedProductsController(ILogger<OrderedProductsController> logger, IOrderedProductRepository orderProductRepository, IMapper mapper)
        {
            _logger = logger;
            _orderProductRepository = orderProductRepository;
            _mapper = mapper;
        }

        // GET: api/v1.0/OrderedProducts
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

        // GET: api/v1.0/OrderedProducts/5
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

        // POST: api/v1.0/OrderedProducts
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

        // PUT: api/v1.0/OrderedProducts/5
        [HttpPut("{orderProductId}")]
        public async Task<ActionResult<OrderedProduct>> Update(int orderProductId, [FromBody] OrderedProductDTO updatedOrderProduct)
        {
            try
            {
                var orderProduct = await _orderProductRepository.Get(orderProductId);

                if (orderProduct == null)
                {
                    return BadRequest($"OrderProduct with id {orderProductId} was not found.");
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

        // DELETE: api/v1.0/OrderedProducts/5
        [HttpDelete("{orderProductId}")]
        public async Task<ActionResult<OrderedProductDTO>> Delete(int orderProductId)
        {
            try
            {

                var orderProduct = await _orderProductRepository.Get(orderProductId);

                if (orderProduct == null)
                {
                    return BadRequest($"OrderProduct with id {orderProductId} was not found.");
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
    }
}
