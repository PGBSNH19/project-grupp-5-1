using System;
using System.IO;
using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Frontend.Services.Interfaces
{
    public interface IImageService
    {
        Task<bool> UploadImages(List<Image> images, int productId);

        Task<Uri> UploadFileToAzureStorage(Stream stream, string container, string fileName);

        string ReadFileFromStorage(string fileName);

        Task<List<Image>> GetAllDefaultImages();

        Task<List<Image>> GetImagesByProductId(int productId);

        Task DeleteImage(string imageName);
    }
}