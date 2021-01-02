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
        public IProductService productService { get; set; }
        public Product product { get; set; } = new Product();
        
      
        public IEnumerable<Product> products { get; set; }
      

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
            product = new Product
            {
             
                ProductCategoryId = 1
            };

            products = (await productService.GetProducts()).ToList();
            var prices = (await productService.GetPrices()).ToList();
            if (prices.Count != 0)
            {
                foreach (var product in products)
            {

                var price = prices.Where(x => x.ProductId == product.Id).FirstOrDefault();
                product.Price = price.Price;
                product.SalePrice = price.SalePrice;

            }

            }
            

        }

        protected async Task HandleValidSubmit()
        {
            var result = await productService.AddProducts(product,ProductPrice,SalePrice);

            if (result != null)
            {
                NavigationManager.NavigateTo("/manageproducts");
            }
        }
    }
}