using System;
using System.Linq;
using Frontend.Models;
using Frontend.Services;
using Frontend.Componenets;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace frontend.Pages
{
    public class AddProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
              
        [Inject]
        public IImageService imageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Product> products { get; set; }     
         
        public Product Product { get; set; } = new Product();

        public string ProductCatId { get; set; }
        
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        
        [Parameter]
        public string Id { get; set; }

        public DropZoneBase child;

        protected async override Task OnInitializedAsync()
        {
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();

            products = (await ProductService.GetProducts()).ToList();
        }

        protected async Task HandleValidSubmit()
        {
            if(ProductCatId == null) { ProductCatId = "1"; }
            Product.ProductCategoryId = int.Parse(ProductCatId);
            var result = await ProductService.AddProducts(Product, Product.Price);

            if (result != null)
            {
                await imageService.UploadImages(child.Images, result.Id);
                NavigationManager.NavigateTo("/manageproducts");
            }
        }
    }
}