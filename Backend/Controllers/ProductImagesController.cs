using System;
using AutoMapper;
using System.Linq;
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
    public class ProductImagesController : ControllerBase
    {
        private readonly ILogger<ProductImagesController> _logger;
        private readonly IProductImageRepository _productImageRepository;

        private readonly IMapper _mapper;

        public ProductImagesController(ILogger<ProductImagesController> logger, IProductImageRepository productImageRepository, IMapper mapper)
        {
            _logger = logger;
            _productImageRepository = productImageRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all existing images.
        /// </summary>
        /// <returns>A list of all existing images.</returns>
        /// <response code="200">Returns a list of existing images.</response>
        /// <response code="404">There are no images stored in the database.</response>
        /// <response code="500">The API caught an exception when attempting to fetch images.</response>
        [HttpGet]
        public async Task<ActionResult<ProductImageDTO[]>> GetAll()
        {
            try
            {
                var images = await _productImageRepository.GetAll();

                if (images == null)
                {
                    return NotFound($"Could not find any images.");
                }

                _logger.LogInformation($"Fetching all images from the database.");
                var mappedResults = _mapper.Map<IList<ProductImageDTO>>(images);
                return Ok(mappedResults);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves a image by its Id.
        /// </summary>
        /// <param name="productImageId">The Id of the requested image.</param>
        /// <returns>The image which has the specified Id.</returns>
        /// <response code="200">Returns the image which matched the given Id.</response>
        /// <response code="404">No image was found which matched the given Id.</response>
        /// <response code="500">The API caught an exception when attempting to fetch an image.</response>
        [HttpGet("{productImageId}")]
        public async Task<ActionResult<ProductImageDTO>> GetById(int productImageId)
        {
            try
            {
                var image = await _productImageRepository.Get(productImageId);

                if (image == null)
                {
                    return NotFound($"Image with id {productImageId} was not found.");
                }

                _logger.LogInformation($"Fetching the image of Id number: {productImageId} from the database.");
                var mappedResult = _mapper.Map<ProductImageDTO>(image);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves all images by their ProductId.
        /// </summary>
        /// <param name="productId">The Id of the requested product.</param>
        /// <returns>The images which has the specified ProductId.</returns>
        /// <response code="200">Returns the images which matched the given ProductId.</response>
        /// <response code="404">No images was found which matched the given ProductId.</response>
        /// <response code="500">The API caught an exception when attempting to fetch images.</response>
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ProductImageDTO[]>> GetImagesByProductId(int productId)
        {
            try
            {
                var images = (await _productImageRepository.GetAll()).Where(p => p.ProductId == productId);

                if (images == null)
                {
                    return NotFound($"No images with ProductId {productId} was found.");
                }

                _logger.LogInformation($"Fetching all images of ProductId number: {productId} from the database.");
                var mappedResult = _mapper.Map<ProductImageDTO[]>(images);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"API Failure: {e.Message}");
            }
        }

        /// <summary>
        /// Adds a new image.
        /// </summary>
        /// <param name="productImage">The new image object to be added.</param>
        /// <returns>The image object which has been added.</returns>
        /// <response code="200">Returns the new image which has been added.</response>
        /// <response code="400">The API failed to save the new image to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an image.</response>
        [HttpPost]
        public async Task<ActionResult<ProductImageDTO>> Add([FromBody] ProductImageDTO productImage)
        {
            try
            {
                var mappedResult = _mapper.Map<ProductImage>(productImage);
                await _productImageRepository.Add(mappedResult);

                if (await _productImageRepository.Save())
                {
                    _logger.LogInformation($"Inserting an new image to the database.");
                    return Ok(_mapper.Map<ProductImageDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to add the image. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates an existing image.
        /// </summary>
        /// <param name="productImageId">The Id of the requested image which will be updated.</param>
        /// <param name="updatedProductImage">The new details of the image object.</param>
        /// <returns>The image object with its updated details.</returns>
        /// <response code="200">Returns the image which has been updated.</response>
        /// <response code="404">No image was found which matched the given Id.</response>
        /// <response code="400">The API failed to save the updated image to the database.</response>
        /// <response code="500">The API caught an exception when attempting to save an image.</response>
        [HttpPut("{productImageId}")]
        public async Task<ActionResult<ProductImage>> Update(int productImageId, [FromBody] ProductImageDTO updatedProductImage)
        {
            try
            {
                var image = await _productImageRepository.Get(productImageId);

                if (image == null)
                {
                    return NotFound($"Image with id {productImageId} was not found.");
                }

                var mappedResult = _mapper.Map(updatedProductImage, image);
                mappedResult.Id = productImageId;
                _productImageRepository.Update(mappedResult);

                if (await _productImageRepository.Save())
                {
                    _logger.LogInformation($"Updating the image of Id number: {productImageId} in the database.");
                    return Ok(_mapper.Map<ProductImageDTO>(mappedResult));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to update the image. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes an existing image.
        /// </summary>
        /// <param name="productImageId">The Id of the image which needs to be deleted.</param>
        /// <returns>The deleted image object.</returns>
        /// <response code="200">Returns the image which has been deleted.</response>
        /// <response code="404">No image was found which matched the given Id.</response>
        /// <response code="400">The API failed to save changes to database after deleting the image.</response>
        /// <response code="500">The API caught an exception when attempting to delete an image.</response>
        [HttpDelete("{productImageId}")]
        public async Task<ActionResult<ProductImageDTO>> Delete(int productImageId)
        {
            try
            {
                var productImage = await _productImageRepository.Get(productImageId);

                if (productImage == null)
                {
                    return NotFound($"Image with id {productImageId} was not found.");
                }

                _productImageRepository.Remove(productImage);

                if (await _productImageRepository.Save())
                {
                    _logger.LogInformation($"Deleting the image of Id number: {productImageId} from the database.");
                    return Ok(_mapper.Map<ProductImageDTO>(productImage));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the image. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes an existing image by image name.
        /// </summary>
        /// <param name="imageName">The name of the image which needs to be deleted.</param>
        /// <returns>The deleted image object.</returns>
        /// <response code="200">Returns the image which has been deleted.</response>
        /// <response code="404">No image was found which matched the given name.</response>
        /// <response code="400">The API failed to save changes to database after deleting the image.</response>
        /// <response code="500">The API caught an exception when attempting to delete an image.</response>
        [HttpDelete("deleteImage/{imageName}")]
        public async Task<ActionResult<ProductImageDTO>> DeleteByImageName(string imageName)
        {
            try
            {
                var productImage = (await _productImageRepository.GetAll()).Where(p => p.ImageName == imageName).FirstOrDefault();

                if (productImage == null)
                {
                    return NotFound($"Image with name {imageName} was not found.");
                }

                _productImageRepository.Remove(productImage);

                if (await _productImageRepository.Save())
                {
                    _logger.LogInformation($"Deleting the image : {imageName} from the database.");
                    return Ok(_mapper.Map<ProductImageDTO>(productImage));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Failed to remove the image. Exception thrown when attempting to add data to the database: {e.Message}");
            }
            return BadRequest();
        }
    }
}