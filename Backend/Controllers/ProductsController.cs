﻿using AutoMapper;
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
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
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
        public async Task<ActionResult<ProductDTO[]>> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAll();

                if (products == null)
                {
                    return NotFound($"Could not find any products.");
                }

                var mappedResult = _mapper.Map<ProductDTO[]>(products);

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
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                var mappedResult = _mapper.Map<ProductDTO>(product);

                return Ok(mappedResult);
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
        /// <param name="product">The new product object to be added.</param>
        /// <returns>The product object which has been added.</returns>
        /// <response code="201">Returns details of the new product which has been added.</response>
        /// <response code="400">The API failed to save the new product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a product.</response>    
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO product)
        {
            try
            {
                var mappedResult = _mapper.Map<Product>(product);

                await _productRepository.Add(mappedResult);
                try
                {
                    await _productRepository.Save();
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
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                _productRepository.Remove(product);

                try
                {
                    await _productRepository.Save();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }

                var mappedResult = _mapper.Map<ProductDTO>(product);
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
        /// <param name="productDTO">The new details of the product object.</param>
        /// <returns>The product object with its updated details.</returns>
        /// <response code="200">Returns the product which has been updated.</response>
        /// <response code="404">No product was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated product to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save a product.</response>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, ProductDTO productDTO)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                {
                    return NotFound($"Product with id {id} was not found.");
                }

                var mappedResult = _mapper.Map(productDTO, product);

                _productRepository.Update(mappedResult);

                if (await _productRepository.Save())
                {
                    return Ok(_mapper.Map<ProductDTO>(mappedResult));
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