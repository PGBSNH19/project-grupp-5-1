using AutoMapper;
using Backend.DTO;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class ProductsPricesController : ControllerBase
    {
        private readonly ILogger<ProductsPricesController> _logger;
        private readonly IProductsPricesRepository _productsPricesRepository;
        private readonly IMapper _mapper;

        public ProductsPricesController(ILogger<ProductsPricesController> logger, IProductsPricesRepository productsPricesRepository, IMapper mapper)
        {
            _logger = logger;
            _productsPricesRepository = productsPricesRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all existing products.
        /// </summary>
        /// <returns>A list of all existing products.</returns>
        /// <response code="200">Returns a list of existing products.</response>
        /// <response code="404">There are no products stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch products.</response>  
        [HttpGet]
        public async Task<ActionResult<ProductPriceDTO[]>> GetAll()
        {
            try
            {
                var products = await _productsPricesRepository.GetAll();

                if (products == null)
                {
                    return NotFound($"Could not find any products.");
                }

                var mappedResult = _mapper.Map<ProductPriceDTO[]>(products);

                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves a product by its Id.
        /// </summary>
        /// <param name="id">The Id of the requested product.</param>
        /// <returns>The product which has the specified Id.</returns>
        /// <response code="200">Returns the product which matched the given Id.</response>
        /// <response code="404">No product was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch a product.</response>    
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductPriceDTO>> GetById(int id)
        {
            try
            {
                var product = await _productsPricesRepository.Get(id);

                if (product == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                var mappedResult = _mapper.Map<ProductPriceDTO>(product);

                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to get the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

        [HttpGet("price/{productId:int}")]
        public async Task<ActionResult<decimal>> GetLatestPriceByProductId(int productId)
        {
            try
            {
                var price = await _productsPricesRepository.GetLatestPrice(productId);

                if (price == 0)
                {
                    return NotFound($"Product with id {productId} has no price.");
                }

                return Ok(price);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to get the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }


        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productPrice">The new product object to be added.</param>
        /// <returns>The product object which has been added.</returns>
        /// <response code="201">Returns details of the new product which has been added.</response>
        /// <response code="400">The API failed to save the new product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a product.</response>    
        [HttpPost]
        public async Task<ActionResult<ProductPriceDTO>> PostProductPrice(ProductPriceDTO productPrice)
        {
            try
            {
                var mappedResult = _mapper.Map<ProductPrice>(productPrice);

                await _productsPricesRepository.Add(mappedResult);
                try
                {
                    await _productsPricesRepository.Save();
                    return CreatedAtAction(nameof(GetById), new { id = mappedResult.Id },
                    mappedResult);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

        /// <summary>
        /// Deletes an existing product.
        /// </summary>
        /// <param name="id">The Id of the product which needs to be deleted.</param>
        /// <returns>The deleted product object.</returns>
        /// <response code="200">Returns the product which has been deleted.</response>
        /// <response code="404">No product was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the product.</response>
        /// <response code="500">The API caught an exception when attempting to delete a product.</response>  
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductPriceDTO>> Delete(int id)
        {
            try
            {
                var product = await _productsPricesRepository.Get(id);

                if (product == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                _productsPricesRepository.Remove(product);

                try
                {
                    await _productsPricesRepository.Save();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }

                var mappedResult = _mapper.Map<ProductPriceDTO>(product);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The Id of the requested product which will be updated.</param>
        /// <param name="productPriceDTO">The new details of the product object.</param>
        /// <returns>The product object with its updated details.</returns>
        /// <response code="200">Returns the product which has been updated.</response>
        /// <response code="404">No product was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a product.</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductPriceDTO>> Update(int id, ProductPriceDTO productPriceDTO)
        {
            try
            {
                var productPrice = await _productsPricesRepository.Get(id);

                if (productPrice == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                var mappedResult = _mapper.Map(productPriceDTO, productPrice);

                _productsPricesRepository.Update(mappedResult);

                if (await _productsPricesRepository.Save())
                {
                    return Ok(_mapper.Map<ProductPriceDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to update the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }
    }
}