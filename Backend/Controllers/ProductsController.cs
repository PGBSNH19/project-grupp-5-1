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

        [HttpGet]
        public async Task<ActionResult<ProductDTO[]>> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAll();

                if (products == null)
                {
                    return NotFound($"Could not find any products");
                }

                var mappedResult = _mapper.Map<ProductDTO[]>(products);

                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

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
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
        }

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
                    return BadRequest();
                }

                var mappedResult = _mapper.Map<ProductDTO>(product);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the product. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                var product = await _productRepository.GetProductById(id);

                if (product == null)
                {
                    return BadRequest($"Product with id {id} was not found.");
                }

                var mappedResult = _mapper.Map(productDTO, product);

                _productRepository.Update(mappedResult);

                if (await _productRepository.Save())
                {
                    return NoContent();
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