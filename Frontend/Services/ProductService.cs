using System.Net.Http;
using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;

namespace Frontend.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;


        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this._configuration = configuration;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] + "api/v1.0/products");
        }

        public async Task<IEnumerable<Product>> SearchProducts(string productName)
        {
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] 
                + $"api/v1.0/products/search/?productname={productName}");
        }


        public async Task<Product> GetProductById(int id)
        {
            return await httpClient.GetJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{id}");
        }

        public async Task<IEnumerable<ProductPrice>> GetAllPrices()
        {
            return await httpClient.GetJsonAsync<IEnumerable<ProductPrice>>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices");
        }

        public async Task<decimal> GetLatestPriceByProductId(int id)
        {
            return await httpClient.GetJsonAsync<decimal>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices/price/{id}");
        }

        public async Task<ProductPrice> GetPriceByProductId(int id)
        {
            return await httpClient.GetJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices/product/{id}");
        }

        public async Task<Product> AddProducts(Product product, decimal productPrice)
        {
            try
            {
             var result = await httpClient.PostJsonAsync<Product>(_configuration["ApiHostUrl"] + "api/v1.0/products", product);
                if (result != null)
                {
                    ProductPrice price = new ProductPrice();
                    price.Price = productPrice;
                    price.SalePrice = 0;
                    price.DateChanged = DateTime.Now;
                    price.ProductId = result.Id;

                    await httpClient.PostJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + "api/v1.0/productsprices", price);
                    return result;
                }
                return null;

            }
            catch (System.Exception)
            {

                return null;
            }
            
        }

        public async Task<Product> Update(Product product, decimal productPrice, decimal salePrice)
        {
          //return await httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);

            try
            {
                var result = await httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);
                if (result != null)
                {
                    ProductPrice price = new ProductPrice();
                    price.Price = productPrice;
                    price.SalePrice = salePrice;
                    price.DateChanged = DateTime.Now;
                    price.ProductId = product.Id;

                 var respons =  await httpClient.PostJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + "api/v1.0/productsprices", price);
                return result;
                }
                return null;

            }
            catch (System.Exception)
            {

                return null;
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategories()
        {
            return await httpClient.GetJsonAsync<ProductCategory[]>(_configuration["ApiHostUrl"] + $"api/v1.0/products/categories");
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int id)
        {
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] + $"api/v1.0/products/categories/{id}");
        }
    }
}