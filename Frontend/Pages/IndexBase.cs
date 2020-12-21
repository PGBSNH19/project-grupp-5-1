using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<Product> products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProducts()).ToList();
        }
    }
}