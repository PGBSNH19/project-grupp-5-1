using System.Linq;
using Frontend.Models;
using Frontend.Services;
using Frontend.Componenets;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

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

        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public string ProductCatId { get; set; }

        [Parameter]
        public string Id { get; set; }

        public DropZoneBase child;

        protected async override Task OnInitializedAsync()
        {
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();

            Product = new Product()
            {
                ProductCategoryId = 1,
                SalePrice = 0
            };
        }

        protected async Task HandleValidSubmit()
        {
            if (ProductCatId == null) { ProductCatId = "1"; }
            Product.ProductCategoryId = int.Parse(ProductCatId);
            var result = await ProductService.AddProducts(Product, Product.Price);

            if (result != null)
            {
                if (await imageService.UploadImages(child.Images, result.Id))
                {
                    NavigationManager.NavigateTo("/manageproducts");
                }
                else
                {
                    await ProductService.DeleteProduct(result.Id);
                } 
            }
        }
    }
}