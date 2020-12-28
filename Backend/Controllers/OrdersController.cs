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
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1.0/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(ILogger<OrdersController> logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all existing orders.
        /// </summary>
        /// <returns>A list of all existing orders.</returns>
        /// <response code="200">Returns a list of existing orders.</response>
        /// <response code="404">There are no orders stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch orders.</response>    
        [HttpGet]
        public async Task<ActionResult<OrderDTO[]>> GetAll()
        {
            try
            {
                var orders = await _orderRepository.GetAll();

                if (orders == null)
                {
                    return NotFound($"Could not find any orders.");
                }

                _logger.LogInformation($"Fetching all orders from the database.");
                var mappedResults = _mapper.Map<IList<OrderDTO>>(orders);
                return Ok(mappedResults);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves a order by its Id.
        /// </summary>
        /// <param name="orderId">The Id of the requested order.</param>
        /// <returns>The order which has the specified Id.</returns>
        /// <response code="200">Returns the order which matched the given Id.</response>
        /// <response code="404">No order was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch an order.</response>    
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetById(int orderId)
        {
            try
            {
                var order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    return NotFound($"Order with id {orderId} was not found.");
                }

                _logger.LogInformation($"Fetching the order of Id number: {orderId} from the database.");
                var mappedResult = _mapper.Map<OrderDTO>(order);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="order">The new order object to be added.</param>
        /// <returns>The order object which has been added.</returns>
        /// <response code="200">Returns the new order which has been added.</response>
        /// <response code="400">The API failed to save the new order to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an order.</response>    
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Add([FromBody] OrderDTO order)
        {
            try
            {
                var mappedResult = _mapper.Map<Order>(order);
                await _orderRepository.Add(mappedResult);

                if (await _orderRepository.Save())
                {
                    _logger.LogInformation($"Inserting an new order to the database.");
                    return Ok(_mapper.Map<OrderDTO>(mappedResult));
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
        /// Updates an existing order.
        /// </summary>
        /// <param name="orderId">The Id of the requested order which will be updated.</param>
        /// <param name="updatedOrder">The new details of the order object.</param>
        /// <returns>The order object with its updated details.</returns>
        /// <response code="200">Returns the order which has been updated.</response>
        /// <response code="404">No order was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated order to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an order.</response>  
        [HttpPut("{orderId}")]
        public async Task<ActionResult<Order>> Update(int orderId, [FromBody] OrderDTO updatedOrder)
        {
            try
            {
                var order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    return NotFound($"Order with id {orderId} was not found.");
                }

                var mappedResult = _mapper.Map(updatedOrder, order);
                mappedResult.Id = orderId;
                _orderRepository.Update(mappedResult);

                if (await _orderRepository.Save())
                {
                    _logger.LogInformation($"Updating the order of Id number: {orderId} in the database.");
                    return Ok(_mapper.Map<OrderDTO>(mappedResult));
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
        /// Deletes an existing order.
        /// </summary>
        /// <param name="orderId">The Id of the order which needs to be deleted.</param>
        /// <returns>The deleted order object.</returns>
        /// <response code="200">Returns the order which has been deleted.</response>
        /// <response code="404">No order was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the order.</response>
        /// <response code="500">The API caught an exception when attempting to delete an order.</response>  
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<OrderDTO>> Delete(int orderId)
        {
            try
            {
                var order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    return NotFound($"Order with id {orderId} was not found.");
                }

                _orderRepository.Remove(order);

                if (await _orderRepository.Save())
                {
                    _logger.LogInformation($"Deleting the order of Id number: {orderId} from the database.");
                    return Ok(_mapper.Map<OrderDTO>(order));
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
