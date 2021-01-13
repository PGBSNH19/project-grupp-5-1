using System;
using System.Linq;
using Frontend.Models;
using Frontend.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IImageService ImageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Product> products { get; set; }
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public List<Image> Images { get; set; }

        public string ProductSearchQuery { get; set; }
        public string ProductCategoryId { get; set; } = "0";
        public string MinPrice { get; set; } = "0";
        public string MaxPrice { get; set; } = "0";

        public int Rea { get; set; }

        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProducts()).ToList();
            products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();
            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                product.ProductCategoryName = ProductCategories.Where(p => p.Id == product.ProductCategoryId).SingleOrDefault()?.CategoryName;

                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
                    product.Price = getProductPrices.Price;
                    product.SalePrice = getProductPrices.SalePrice;
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                    product.CurrentPrice = 0;
                }
            }
            Images = await ImageService.GetAllDefaultImages();
        }

        protected async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(ProductSearchQuery))
            {
                products = (await ProductService.GetProducts()).ToList();
                products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            }
            else
            {
                products = (await ProductService.SearchProducts(ProductSearchQuery)).ToList();
                products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            }

            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
                    var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                    product.Price = getProductPrices.Price;
                    product.SalePrice = getProductPrices.SalePrice;
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                    product.CurrentPrice = 0;
                }
            }
        }

        protected List<Product> GetFeaturedProducts()
        {
            var s = products.Where(x => x.IsFeatured).ToList();
            return products.Where(x => x.IsFeatured).ToList();
        }

        protected async Task FilterByProductCategory(int id)
        {
            if (id == 0)
            {
                products = await ProductService.GetProducts();
                products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            }
            else
            {
                products = await ProductService.GetProductsByCategoryId(id);
                products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            }

            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
                    var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                    product.Price = getProductPrices.Price;
                    product.SalePrice = getProductPrices.SalePrice;
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                    product.CurrentPrice = 0;
                }
            }
        }

        public async Task FilterByPriceRange(int min, int max)
        {
            products = await ProductService.GetProducts();
            products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
                    var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                    product.Price = getProductPrices.Price;
                    product.SalePrice = getProductPrices.SalePrice;
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                    product.CurrentPrice = 0;
                }
            }

            if (max > 0)
            {
                products = products.Where(c => c.CurrentPrice >= min && c.CurrentPrice <= max);
                products = products.Where(x => x.IsAvailable == true && x.Stock >= 1).ToList();
            }
        }

        public void OpenDetails(int productId)
        {
            NavigationManager.NavigateTo("productdetails/" + productId);
        }
    }
}