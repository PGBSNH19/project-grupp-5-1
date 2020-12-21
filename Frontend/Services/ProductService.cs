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
            var s = _configuration["ApiHostUrl"];
            return await httpClient.GetJsonAsync<Product[]>(_configuration["ApiHostUrl"] + "api/v1.0/products");
        }

        public async Task<Product> GetProductById(int id)
        {
            return await httpClient.GetJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{id}");
        }
    }
}