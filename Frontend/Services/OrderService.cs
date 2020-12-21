using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient httpClient;

        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await httpClient.GetJsonAsync<Order[]>("api/v1.0/orders");
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await httpClient.GetJsonAsync<Order>($"api/v1.0/orders/{id}");
        }

        public async Task<IEnumerable<OrderedProduct>> GetOrderedProducts()
        {
            return await httpClient.GetJsonAsync<OrderedProduct[]>("api/v1.0/orderedproducts");
        }

        public async Task<OrderedProduct> GetOrderedProductById(int id)
        {
            return await httpClient.GetJsonAsync<OrderedProduct>($"api/v1.0/orderedproducts/{id}");
        }
    }
}