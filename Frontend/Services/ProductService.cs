using System.Net.Http;
using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

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
        public async Task<Product> AddProducts(Product product)
        {
            //product.Id = null;
            return await httpClient.PostJsonAsync<Product>(_configuration["ApiHostUrl"] + "api/v1.0/products", product);
        }

        public async Task<Product> Update(Product product)
        {
            //product.Id = null;
            return await httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllProductCategories()
        {
            return await httpClient.GetJsonAsync<ProductCategory[]>(_configuration["ApiHostUrl"] + $"api/v1.0/products/categories");
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int id)
        {
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] + $"api/v1.0/products/categories/{id}");
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRange(int min, int max)
        {
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] + $"api/v1.0/products/pricerange/{min},{max}");
        }


    }
}