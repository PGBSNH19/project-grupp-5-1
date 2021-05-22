using System;
using System.IO;
using System.Linq;
using Frontend.Models;
using BlazorInputFile;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.Componenets
{
    public class DropZoneBase : ComponentBase
    {

        [Inject]
        public IImageService imageService { get; set; }

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
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ShowExistsFiles(ProductId);
                StateHasChanged();
            }
        }

        protected async Task<List<Image>> AddFiles(IFileListEntry[] files)
        {
            if (files == null)
            {
                return null;
            }
            else
            {
                foreach (var file in files)
                {
                    if (file.Name.Contains("png") || file.Name.Contains("jpg") || file.Name.Contains("gif"))
                    {
                        Image image = new Image();

                        image.ImageFile = await file.ToImageFileAsync(file.Type, 512, 512);
                        var stream = new MemoryStream();
                        await image.ImageFile.Data.CopyToAsync(stream);

                        image.ImageDataURL = $"data:{file.Type};base64,{Convert.ToBase64String(stream.ToArray())}";

                        Images.Add(image);
                    }
                    else
                    {
                        await JsRuntime.InvokeAsync<bool>("confirm", "Your file is not acceptable type..");
                    }
                }
                StateHasChanged();
                return Images;
            }
        }

        public async Task ShowExistsFiles(int productId)
        {
            if (productId != 0)
            {
                Images = await imageService.GetImagesByProductId(productId);
            }
        }

        protected async void RemoveFile(Image image)
        {
            bool confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
            if (confirmed && Images.Contains(image))
            {
                if(image.ImageURL != null)
                {
                    var name = Path.GetFileName(image.ImageURL);
                    await imageService.DeleteImage(name);
                }
                Images.Remove(image);
                StateHasChanged();
            }
        }

        public Image CheckboxChange(Image productImage)
        {
            foreach (var image in Images)
            {
                if (image.IsDefault == true && image.Id != productImage.Id)
                {
                    image.IsDefault = false;
                }
            }
            productImage.IsDefault = !productImage.IsDefault;
            StateHasChanged();
            return productImage;
        }
    }
}