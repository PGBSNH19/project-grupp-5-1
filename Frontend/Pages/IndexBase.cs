using Frontend.Models;
using Frontend.Services;
using Frontend.Services.Interfaces;
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
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public string ProductSearchQuery { get; set; }
        public string ProductCategoryId { get; set; } = "0";
        public string MinPrice { get; set; } = "0";
        public string MaxPrice { get; set; } = "0";


        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProducts()).ToList();
            GetProductPrices = await ProductService.GetAllPrices();

            foreach(var product in products)
            {
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
            ProductCategories = await ProductService.GetAllProductCategories();
        }

        protected async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(ProductSearchQuery))
                products = (await ProductService.GetProducts()).ToList();
            else
                products = (await ProductService.SearchProducts(ProductSearchQuery)).ToList();
        }

        protected List<Product> GetFeaturedProducts()
        {
            return products.Where(x => x.IsFeatured).ToList();
        }

        protected async Task FilterByProductCategory(int id)
        {
            if (id == 0)
                products = await ProductService.GetProducts();
            else
                products = await ProductService.GetProductsByCategoryId(id);
        }

        protected async Task FilterByPriceRange(int min, int max)
        {
            products = await ProductService.GetProductsByPriceRange(min, max);
        }
    }
}