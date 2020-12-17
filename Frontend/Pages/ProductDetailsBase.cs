using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        public Product product { get; set; } = new Product();

        [Inject]
        public IProductService ProductService { get; set; }

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            //Id = Id ?? "1";
            product = await ProductService.GetProductById(int.Parse(Id));
        }
    }
}