using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace frontend.Pages
{
    public class AddProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public Product Product { get; set; } = new Product();
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public IEnumerable<Product> products { get; set; }
        public string ProductCatId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


      
        
        
        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();

            Product = new Product()
            { 
                ProductCategoryId = 1
            };

        }

        protected async Task HandleValidSubmit()
        {
            Product.ProductCategoryId = int.Parse(ProductCatId);
            var result = await ProductService.AddProducts(Product);

            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}