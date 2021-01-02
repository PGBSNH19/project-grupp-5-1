using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var prices = (await ProductService.GetPrices()).ToList();
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
    }
}