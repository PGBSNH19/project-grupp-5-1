using Frontend.Models;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using System.Linq;
using System;

namespace Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly NavigationManager _NavigationManager;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorageService;
        private readonly IJSRuntime _jSRuntime;

        public OrderService(NavigationManager NavigationManager, HttpClient httpClient, IConfiguration configuration, ILocalStorageService localStorageService, IJSRuntime jSRuntime)
        {
            _NavigationManager = NavigationManager;
            _httpClient = httpClient;
            _configuration = configuration;
            _localStorageService = localStorageService;
            _jSRuntime = jSRuntime;
        }

        public async Task<IEnumerable<ProductInBasket>> GetBasketProducts()
        {
            return await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket");
        }

        public async Task IncreaseProductToBasket(Product product)
        {
            bool basketExists = await _localStorageService.ContainKeyAsync("customer-basket");
            var basket = basketExists ? await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket") : new List<ProductInBasket>();

            ProductInBasket productInBasket = basket.FirstOrDefault(x => x.Product.Id == product.Id);

            if (productInBasket.Amount >= product.Stock)
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"You can't order more than {product.Stock} of this product.");
            }
            else
            {
                productInBasket.Amount++;
            }

            await _localStorageService.SetItemAsync("customer-basket", basket);
            _NavigationManager.NavigateTo("/OrderPage", true);
        }

        public async Task DecreaseProductToBasket(Product product)
        {
            bool basketExists = await _localStorageService.ContainKeyAsync("customer-basket");
            var basket = basketExists ? await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket") : new List<ProductInBasket>();

            ProductInBasket productInBasket = basket.FirstOrDefault(x => x.Product.Id == product.Id);

            if (productInBasket.Amount < 1)
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"You can't order less than one product.");
            }
            else
            {
                productInBasket.Amount--;
            }

            await _localStorageService.SetItemAsync("customer-basket", basket);
            _NavigationManager.NavigateTo("/OrderPage", true);
        }

        public async Task DeleteProductFromBasket(ProductInBasket product)
        {
            bool basketExists = await _localStorageService.ContainKeyAsync("customer-basket");
            var basket = basketExists ? await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket") : new List<ProductInBasket>();

            int productIndex = basket.FindIndex(x => x.Product.Id == product.Product.Id);

            if (product != null)
            {
                basket.RemoveAt(productIndex);
            }
            else
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"This product is not in your basket.");
            }

            await _localStorageService.SetItemAsync("customer-basket", basket);

            if (basket.Count == 0)
            {
                await _localStorageService.ClearAsync();
            }
            _NavigationManager.NavigateTo("/OrderPage", true);
        }

        public async Task CreateOrder(IEnumerable<ProductInBasket> basketProducts)
        {
            Order order = new Order();

            order.DateRegistered = DateTime.Now;
            order.CouponId = (int?)null;
            order.UserId = 1;

            var newOrder = await _httpClient.PostJsonAsync<Order>(_configuration["ApiHostUrl"] + "api/v1.0/orders", order);

            if (newOrder != null)
            {
                foreach (var basketProduct in basketProducts)
                {
                    OrderedProduct orderedProduct = new OrderedProduct();

                    orderedProduct.Amount = basketProduct.Amount;
                    orderedProduct.OrderId = newOrder.Id;
                    orderedProduct.ProductId = basketProduct.Product.Id;

                    await _httpClient.PostJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts", orderedProduct);

                    var product = await _httpClient.GetJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{basketProduct.Product.Id}");

                    product.Stock -= basketProduct.Amount;

                    await _httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);
                }

                await _localStorageService.ClearAsync();
                await _jSRuntime.InvokeAsync<bool>("confirm", $"Thank you for your order. You are welcome back...");
                _NavigationManager.NavigateTo("/");
            }
            else
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"Sorry, we can not send this order...");
            }
        }
    }
}