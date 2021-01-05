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
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        [Parameter]
        public string Id { get; set; }

        protected async override Task OnInitializedAsync()
        {
            product = await ProductService.GetProductById(int.Parse(Id));

            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();
            product.ProductCategoryName = ProductCategories.Where(p => p.Id == product.ProductCategoryId).SingleOrDefault()?.CategoryName;
            
            GetProductPrices = await ProductService.GetAllPrices();

            bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
            if (hasFound)
            {
                var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
            }
            else
            {
                product.Price = 0;
                product.SalePrice = 0;
            }
        }
    }
}