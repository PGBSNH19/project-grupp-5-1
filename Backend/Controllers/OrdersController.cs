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

        // GET: api/v1.0/Orders
        [HttpGet]
        public async Task<ActionResult<OrderDTO[]>> GetAll()
        {
            try
            {
                var orders = await _orderRepository.GetAll();

                if (orders == null)
                {
                    return NotFound($"Could not find any order");
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

        // GET: api/v1.0/Orders/5
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

        // POST: api/v1.0/Orders
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

        // PUT: api/v1.0/Orders/5
        [HttpPut("{orderId}")]
        public async Task<ActionResult<Order>> Update(int orderId, [FromBody] OrderDTO updatedOrder)
        {
            try
            {
                var order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    return BadRequest($"Order with id {orderId} was not found.");
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

        // DELETE: api/v1.0/Orders/5
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<OrderDTO>> Delete(int orderId)
        {
            try
            {

                var order = await _orderRepository.Get(orderId);

                if (order == null)
                {
                    return BadRequest($"Order with id {orderId} was not found.");
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
