using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;


        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this._configuration = configuration;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await httpClient.GetJsonAsync<Order[]>(_configuration["ApiHostUrl"] + "api/v1.0/orders");
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await httpClient.GetJsonAsync<Order>(_configuration["ApiHostUrl"] + $"api/v1.0/orders/{id}");
        }

        public async Task<IEnumerable<OrderedProduct>> GetOrderedProducts()
        {
            return await httpClient.GetJsonAsync<OrderedProduct[]>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts");
        }

        public async Task<OrderedProduct> GetOrderedProductById(int id)
        {
            return await httpClient.GetJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + $"api/v1.0/orderedproducts/{id}");
        }
    }
}