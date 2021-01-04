using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace frontend.Pages
{
    public class AddProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
              
        public IEnumerable<Product> products { get; set; }      
        public Product Product { get; set; } = new Product();
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public string ProductCatId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be over 0.")]
        public decimal ProductPrice { get; set; }

        public decimal SalePrice { get; set; }


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
            var result = await ProductService.AddProducts(Product,ProductPrice,SalePrice);
            Product.ProductCategoryId = int.Parse(ProductCatId);

            if (result != null)
            {
                NavigationManager.NavigateTo("/manageproducts");
            }
        }
    }
}