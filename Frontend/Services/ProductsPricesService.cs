using System.Net.Http;
using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using Frontend.Services.Interfaces;

namespace Frontend.Services
{
    public class ProductsPricesService: IProductsPricesService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;

        public ProductsPricesService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this._configuration = configuration;
        }

        public async Task<IEnumerable<ProductPrice>> GetAllPrices()
        {
            return await httpClient.GetJsonAsync<IEnumerable<ProductPrice>>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices");
        }

        public async Task<ProductPrice> GetById(int id)
        {
            return await httpClient.GetJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices/{id}");
        }

        public async Task<decimal> GetLatestPriceByProductId(int productId)
        {
            return await httpClient.GetJsonAsync<decimal>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices/price/{productId}");
        }

        public async Task<ProductPrice> PostProductPrice(ProductPrice productPrice)
        {
            return await httpClient.PostJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + "api/v1.0/productsprices", productPrice);
        }

        public async Task<ProductPrice> UpdatePrice(int id, ProductPrice productPrice)
        {
            return await httpClient.PutJsonAsync<ProductPrice>(_configuration["ApiHostUrl"] + $"api/v1.0/productsprices/{id}", productPrice);
        }
    }
}
