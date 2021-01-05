using System;
using System.IO;
using Frontend.Models;
using BlazorInputFile;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Frontend.Componenets
{
    public class ProductImagesBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        private IWebHostEnvironment _env { get; set; }

        [Parameter]
        public List<Image> Images { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        protected override void OnInitialized()
        {
           Images = new List<Image>();
           ShowExistsFiles(ProductId);
        }

        public async void UploadImagesFromParent(int ProductId)
        {
            await UploadImages(Images, ProductId);
        }

        protected async void AddFiles(IFileListEntry[] files)
        {
            if (files == null)
            {
                return;
            }
            else
            {
                foreach (var file in files)
                {
                    if (file.Name.Contains("png") || file.Name.Contains("jpg") || file.Name.Contains("gif"))
                    {
                        var ms = new MemoryStream();
                        await file.Data.CopyToAsync(ms);
                        string imageBase64Data = Convert.ToBase64String(ms.ToArray());
                        var imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                        var image = new Image();
                        image.ImageDataURL = imageDataURL;
                        image.ImageFile = file;

                        Images.Add(image);
                    }
                }
                StateHasChanged();
            }
        }


        protected async void ShowExistsFiles(int productId)
        {
            if (productId != 0)
            {
                List<string> files = new List<string>();
                string sourceDir = _env.WebRootPath + $"\\Products\\Product-Id-{productId}\\";

                files = Directory.GetFiles(sourceDir).ToList() != null ? Directory.GetFiles(sourceDir).ToList() : files;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var image = new Image();
                        image.ImageDataURL = file;
                        Images.Add(image);
                    }
                } 
            }

            //if (files == null)
            //{
            //    return;
            //}
            //else
            //{
            //    foreach (var file in files)
            //    {
            //        if (file.Name.Contains("png") || file.Name.Contains("jpg") || file.Name.Contains("gif"))
            //        {
            //            var ms = new MemoryStream();
            //            await file.Data.CopyToAsync(ms);
            //            string imageBase64Data = Convert.ToBase64String(ms.ToArray());
            //            var imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

            //            var image = new Image();
            //            image.ImageDataURL = imageDataURL;
            //            image.ImageFile = file;

            //            Images.Add(image);
            //        }
            //    }
            //    StateHasChanged();
            //}
        }

        protected async void RemoveFile(Image image)
        {
            bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
            if (confirmed)
            {
                Images.Remove(image);
                StateHasChanged();
            }
        }

        protected async Task<object> UploadImages(List<Image> images, int productId)
        {
            Dictionary<string, dynamic> results = new Dictionary<string, dynamic>
            {
                {"success", true },
            };

            try
            {
                string sourceDir = _env.WebRootPath + $"\\Products\\Product-Id-{productId}\\";

                if (images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        if (!Directory.Exists(sourceDir))
                        {
                            Directory.CreateDirectory(sourceDir);
                        }

                        var path = Path.Combine(sourceDir + image.ImageFile.Name);
                        var memoryStream = new MemoryStream();
                        if (!File.Exists(path))
                        {
                            await image.ImageFile.Data.CopyToAsync(memoryStream);

                            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                            {
                                memoryStream.WriteTo(fileStream);
                                memoryStream.Flush();
                            } 
                        }
                    }
                    return results;
                }
                else
                {
                    results["success"] = false;
                    results.Add("Message", "Images are not exist to upload...");
                    return results;
                }
            }
            catch (Exception ex)
            {
                results["success"] = false;
                results.Add("Message", ex.Message.ToString());
                return results;
            }
        }
    }
}