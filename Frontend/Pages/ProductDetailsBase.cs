using Frontend.Models;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Inject]
        private IWebHostEnvironment _env { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IImageService ImageService { get; set; }

        public Product product { get; set; } = new Product();

        public IEnumerable<ProductPrice> GetProductPrices { get; set; }

        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public List<Image> Images { get; set; } = new List<Image>();

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
                product.Price = getProductPrices.Price;
                product.SalePrice = getProductPrices.SalePrice;
                product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
            }
            else
            {
                product.Price = 0;
                product.SalePrice = 0;
                product.CurrentPrice = 0;
            }
            Images = await ImageService.GetImagesByProductId(product.Id);
        }
    }
}