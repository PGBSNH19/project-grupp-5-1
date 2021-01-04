using System;
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
using Frontend.Models.Mail;

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
        private List<BuyedProducts> buyedProducts = new List<BuyedProducts>();

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

        public async Task CreateOrder(UserInfo userInfo)
        {
            await _tokenValidator.CheckToken(_httpClient);

            Order order = new Order();

            order.DateRegistered = DateTime.Now;
            order.CouponId = 1;
            order.UserId = 1;

            if (_httpClient.DefaultRequestHeaders.Authorization != null)
            {
                var newOrder = await _httpClient.PostJsonAsync<Order>(_configuration["ApiHostUrl"] + "api/v1.0/orders", order);
                if (newOrder != null)
                {
                    foreach (var basketProduct in userInfo.userBasket)
                    {
                        OrderedProduct orderedProduct = new OrderedProduct()
                        {
                            Amount = basketProduct.Amount,
                            OrderId = newOrder.Id,
                            ProductId = basketProduct.Product.Id,
                        };

                        await _httpClient.PostJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts/", orderedProduct);

                        var product = await _httpClient.GetJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{basketProduct.Product.Id}");

                        product.Stock -= basketProduct.Amount;

                        await _httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);

                        BuyedProducts buyedProduct = new BuyedProducts()
                        {
                            ProductName = product.Name,
                            Amount = basketProduct.Amount,
                            Description = product.Description,
                            Price = 10,
                        };

                        buyedProducts.Add(buyedProduct);
                    }

                    MailRequest orderToSend = new MailRequest()
                    {
                        ToEmail = userInfo.Email,
                        OrderId = newOrder.Id,
                        Subject = "Your order",
                        UserName = userInfo.FirstName + " " + userInfo.LastName,
                        Address = userInfo.Address,
                        City = userInfo.City,
                        ZipCode = userInfo.ZipCode.ToString(),
                        buyedProductsList = buyedProducts
                    };

                    await _httpClient.PostJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts/send/", orderToSend);

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
}