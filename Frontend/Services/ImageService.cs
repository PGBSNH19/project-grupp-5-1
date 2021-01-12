using System;
using System.IO;
using System.Linq;
using Azure.Storage;
using System.Net.Http;
using Frontend.Models;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Frontend.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IOptions<AzureStorageConfig> _options;

        public ImageService(HttpClient httpClient, IOptions<AzureStorageConfig> options, IConfiguration configuration)
        {
            _options = options;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task UploadImages(List<Image> images, int productId)
        {
            if (!images.Any(a => a.IsDefault == true) && images.Count != 0)
            {
                images.FirstOrDefault().IsDefault = true;
            }

            foreach (var image in images)
            {
                if (image.ImageURL == null)
                {
                    //extract just base64 string without data:image/png;base64 (for example)
                    var base64Data = Regex.Match(image.ImageDataURL, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var bytes = Convert.FromBase64String(base64Data);

                    using (var stream = new MemoryStream(bytes))
                    {
                        //assign an unique image name.
                        var extension = image.ImageFile.Name.Substring(image.ImageFile.Name.Length - 4);
                        var fileName = Guid.NewGuid().ToString() + extension;

                        //send the images to Azure blob.
                        var uploadedUri = await UploadFileToAzureStorage(stream, _options.Value.Container, fileName);

                        //send the image names to the database.
                        if (uploadedUri != null)
                        {
                            var productImage = new ProductImageName();
                            productImage.ImageName = fileName;
                            productImage.ProductId = productId;
                            productImage.IsDefault = image.IsDefault;
                            await _httpClient.PostJsonAsync<Image>(_configuration["ApiHostUrl"] + "api/v1.0/productimages", productImage);
                        }
                    }
                }
                else
                {
                    var productImage = new ProductImageName();
                    productImage.ImageName = Path.GetFileName(image.ImageURL);
                    productImage.ProductId = productId;
                    productImage.IsDefault = image.IsDefault;
                    await _httpClient.PutJsonAsync<Image>(_configuration["ApiHostUrl"] + "api/v1.0/productimages/" + image.Id, productImage);
                }
            }
        }

        public async Task<Uri> UploadFileToAzureStorage(Stream stream, string container, string fileName)
        {
            Uri blobUri = new Uri("https://" +
                                  _options.Value.AccountName +
                                  ".blob.core.windows.net/" +
                                  container + "/" + fileName);

            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_options.Value.AccountName, _options.Value.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobUri, storageCredentials);

            // Upload the file
            await blobClient.UploadAsync(stream, true);

            return blobUri;
        }

        public async Task<List<Image>> GetImagesByProductId(int productId)
        {
            List<Image> images = new List<Image>();
            var imageNames =  await _httpClient.GetJsonAsync<ProductImageName[]>(_configuration["ApiHostUrl"] + "api/v1.0/productimages/product/" + productId);
            foreach (var imageName in imageNames)
            {
                var Url = ReadFileFromStorage(imageName.ImageName);

                var image = new Image();
                image.ImageURL = Url;
                image.Id = imageName.Id;
                image.IsDefault = imageName.IsDefault;

                images.Add(image);
            }
            return images;
        }

        public async Task<List<Image>> GetAllDefaultImages()
        {
            List<Image> images = new List<Image>();
            var imageNames = (await _httpClient.GetJsonAsync<ProductImageName[]>(_configuration["ApiHostUrl"] + "api/v1.0/productimages")).Where(p => p.IsDefault == true);
            foreach (var imageName in imageNames)
            {
                var Url = ReadFileFromStorage(imageName.ImageName);

                var image = new Image();
                image.ImageURL = Url;
                image.Id = imageName.Id;
                image.IsDefault = imageName.IsDefault;
                image.ProductId = imageName.ProductId;

                images.Add(image);
            }
            return images;
        }

        public string ReadFileFromStorage(string fileName)
        {
            return new Uri("https://" + _options.Value.AccountName + ".blob.core.windows.net/"  + _options.Value.Container + "/" + fileName).ToString();
        }

        public async Task DeleteImage(string imageName)
        {
            //Delete the imageName from database.
            await _httpClient.SendJsonAsync<Image>(HttpMethod.Delete,_configuration["ApiHostUrl"] + "api/v1.0/productimages/deleteImage/" + imageName, null);

            //Set the connection string variable
            string storageConnectionString = _options.Value.ConnectionString;

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(_options.Value.Container);

            //Delete the Blob
            await container.GetBlockBlobReference(imageName).DeleteAsync();
        }
    }
}
