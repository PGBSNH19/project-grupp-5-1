using Frontend.Models;
using Frontend.Utility;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await httpClient.GetJsonAsync<Product[]>($"{APIRoute.Products}");
        }

        public async Task<Product> GetProductById(int id)
        {
            return await httpClient.GetJsonAsync<Product>($"{APIRoute.Products}/{id}");
        }
        public async Task<Product> AddProducts(Product product)
        {
            //product.Id = null;
            return await httpClient.PostJsonAsync<Product>("api/v1.0/products", product);
        }

    }
}