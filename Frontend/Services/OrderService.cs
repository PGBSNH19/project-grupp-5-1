﻿using System;
using System.Linq;
using Frontend.Models;
using System.Net.Http;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Frontend.Auth;

namespace Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _NavigationManager;

        private readonly IJSRuntime _jSRuntime;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorageService;
        private readonly ITokenValidator _tokenValidator;

        public OrderService(NavigationManager NavigationManager, HttpClient httpClient, IConfiguration configuration, ILocalStorageService localStorageService, IJSRuntime jSRuntime, ITokenValidator tokenValidator)
        {
            _jSRuntime = jSRuntime;
            _httpClient = httpClient;
            _configuration = configuration;
            _tokenValidator = tokenValidator;
            _NavigationManager = NavigationManager;
            _localStorageService = localStorageService;
        }


        public async Task<IEnumerable<ProductInBasket>> GetBasketProducts()
        {
            return await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket");
        }

        public async Task IncreaseProductToBasket(Product product)
        {
            var basket = await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket");

            ProductInBasket productInBasket = basket.FirstOrDefault(x => x.Product.Id == product.Id);

            productInBasket.Amount++;

            await _localStorageService.SetItemAsync("customer-basket", basket);
        }

        public async Task DecreaseProductToBasket(Product product)
        {
            var basket = await _localStorageService.GetItemAsync<List<ProductInBasket>>("customer-basket");

            ProductInBasket productInBasket = basket.FirstOrDefault(x => x.Product.Id == product.Id);

            productInBasket.Amount--;

            await _localStorageService.SetItemAsync("customer-basket", basket);
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
        }

        public async Task CreateOrder(IEnumerable<ProductInBasket> basketProducts)
        {
            await _tokenValidator.CheckToken(_httpClient);

            Order order = new Order();

            order.DateRegistered = DateTime.Now;
            order.CouponId = 1;
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

                await _localStorageService.RemoveItemAsync("customer-basket");
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