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
        public IProductService productService { get; set; }
        public Product product { get; set; } = new Product();

        public IEnumerable<Product> products { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


      
        
        
        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            product = new Product
            {
             
                ProductCategoryId = 1
            };

            products = (await productService.GetProducts()).ToList();
            
        }

        protected async Task HandleValidSubmit()
        {
            var result = await productService.AddProducts(product);

            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}