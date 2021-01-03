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
        

        [Inject]
        public IProductService ProductService { get; set; }

        public Product product { get; set; } = new Product();
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }       

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            //Id = Id ?? "1";
            product = await ProductService.GetProductById(int.Parse(Id));

            GetProductPrices = await ProductService.GetAllPrices();
            bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
            if (hasFound)
            {
                product.Price = await ProductService.GetLatestPriceByProductId(product.Id);
                var saleprice = await ProductService.GetPriceByProductId(product.Id);
                product.SalePrice = saleprice.SalePrice;
            }
            else
            {
                product.Price = 0;
                product.SalePrice = 0;
            }
        }
    }
}