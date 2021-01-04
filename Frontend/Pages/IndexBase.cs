﻿using Frontend.Models;
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
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public string ProductSearchQuery { get; set; }
        public string ProductCategoryId { get; set; } = "0";
        public string MinPrice { get; set; } = "0";
        public string MaxPrice { get; set; } = "0";


        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProducts()).ToList();
            GetProductPrices = await ProductService.GetAllPrices();
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();

            foreach (var product in products)
            {
                product.ProductCategoryName = ProductCategories.Where(p => p.Id == product.ProductCategoryId).SingleOrDefault()?.CategoryName;

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
            }          
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

            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
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
            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var product in products)
            {
                bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
                if (hasFound)
                {
                    product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
                }
                else
                {
                    product.Price = 0;
                    product.SalePrice = 0;
                    product.CurrentPrice = 0;
                }
            }

            if(max > 0 )
                products = products.Where(c => c.CurrentPrice >= min && c.CurrentPrice <= max);
        }
    }
}