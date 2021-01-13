using Blazored.LocalStorage;
using Frontend.Auth;
using Frontend.Models;
using Frontend.Models.Mail;
using Frontend.Services.Interfaces;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _NavigationManager;

        private readonly IMatToaster _toaster;
        private readonly IConfiguration _configuration;
        private readonly ITokenValidator _tokenValidator;
        private readonly ILocalStorageService _localStorageService;

        private List<BuyedProducts> buyedProducts = new List<BuyedProducts>();

        public OrderService(NavigationManager NavigationManager, HttpClient httpClient, IConfiguration configuration,
                            ILocalStorageService localStorageService, ITokenValidator tokenValidator, IMatToaster toaster)
        {
            _toaster = toaster;
            _httpClient = httpClient;
            _configuration = configuration;
            _tokenValidator = tokenValidator;
            _NavigationManager = NavigationManager;
            _localStorageService = localStorageService;
        }

        public async Task<User> GetUserDetails()
        {
            return await _localStorageService.GetItemAsync<User>("user-details");
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
                _toaster.Add($"This product is not exist in your basket.", MatToastType.Danger, "Alert:");
            }

            await _localStorageService.SetItemAsync("customer-basket", basket);

            if (basket.Count == 0)
            {
                await _localStorageService.ClearAsync();
            }
        }

        public async Task CreateOrder(UserInfo userInfo, string couponId)
        {
            await _tokenValidator.CheckToken(_httpClient);

            Order newOrder = new Order();
            OrderedProduct newOrderedProduct = new OrderedProduct();
            Product product = new Product();
            ProductInBasket productInBasket = new ProductInBasket();

            try
            {
                if (_httpClient.DefaultRequestHeaders.Authorization != null)
                {
                    Order order = new Order();

                    order.DateRegistered = DateTime.Now;
                    if (couponId != "0")
                    {
                        order.CouponId = int.Parse(couponId);
                    }
                    order.UserId = (await GetUserDetails()).Id;

                    //The first step is to create the order
                    newOrder = await _httpClient.PostJsonAsync<Order>(_configuration["ApiHostUrl"] + "api/v1.0/orders", order);

                    if (newOrder != null)
                    {
                        foreach (var item in userInfo.userBasket)
                        {
                            productInBasket = item;
                            OrderedProduct orderedProduct = new OrderedProduct()
                            {
                                Amount = productInBasket.Amount,
                                OrderId = newOrder.Id,
                                ProductId = productInBasket.Product.Id,
                            };

                            //The second step is to create the orderProduct
                            newOrderedProduct = await _httpClient.PostJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts/", orderedProduct);

                            if (newOrder != null && newOrderedProduct != null)
                            {
                                product = await MatHttpClientExtension.GetJsonAsync<Product>(_httpClient, _configuration["ApiHostUrl"] + $"api/v1.0/products/{productInBasket.Product.Id}");

                                product.Stock -= productInBasket.Amount;

                                //The third step is to deduct the the purchased amount from the product's stock
                                await _httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);

                                BuyedProducts buyedProduct = new BuyedProducts()
                                {
                                    ProductName = product.Name,
                                    Amount = productInBasket.Amount,
                                    Description = product.Description,
                                    TotalPrice = (decimal)productInBasket.Product.CurrentPrice * productInBasket.Amount,
                                };

                                buyedProducts.Add(buyedProduct);
                            }
                        }

                        string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "W. Europe Standard Time" : "Europe/Stockholm";

                        // Get a TimeZoneInfo object for that time zone
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

                        // Convert the current UTC time to the time in Sweden
                        DateTimeOffset currentTimeInSweden = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzi);

                        MailRequest orderToSend = new MailRequest();

                        orderToSend.ToEmail = userInfo.Email;
                        orderToSend.OrderId = newOrder.Id;
                        orderToSend.Subject = "Your order";
                        orderToSend.UserName = userInfo.FirstName + " " + userInfo.LastName;
                        orderToSend.Address = userInfo.Address;
                        orderToSend.City = userInfo.City;
                        orderToSend.ZipCode = userInfo.ZipCode.ToString();
                        orderToSend.Date = currentTimeInSweden.ToString("F");
                        if (couponId != "0")
                        {
                            var activeDiscount = await MatHttpClientExtension.GetJsonAsync<Coupon>(_httpClient, _configuration["ApiHostUrl"] + $"api/v1.0/Coupons/{couponId}");

                            orderToSend.Discount = (activeDiscount.Discount * 100).ToString("0") + "%";
                            orderToSend.DiscountName = "<strong> Discount: </strong>" + activeDiscount.Code + " give you";
                        }
                        else
                        {
                            orderToSend.Discount = "";
                            orderToSend.DiscountName = "";
                        }
                        orderToSend.TotalPiceWithDiscount = userInfo.TotalPiceWithDiscount;

                        orderToSend.buyedProductsList = buyedProducts;

                        //The fourth step is to send the email to the customer
                        await _httpClient.PostJsonAsync<OrderedProduct>(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts/send/", orderToSend);

                        //The fifth and last step is to remove products from the basket, notify the customer with succes message and nivigate to the home page
                        await _localStorageService.RemoveItemAsync("customer-basket");
                        _toaster.Add($"Thank you for your order. You are welcome back...", MatToastType.Success, "Order Sent:");

                        _NavigationManager.NavigateTo("/");
                    }
                }
            }
            catch (Exception)
            {
                //If something wrong happened when sending the order, first we delete the steps which was already created
                if (newOrderedProduct != null && newOrder != null)
                {
                    product = await MatHttpClientExtension.GetJsonAsync<Product>(_httpClient, _configuration["ApiHostUrl"] + $"api/v1.0/products/{productInBasket.Product.Id}");
                    product.Stock += productInBasket.Amount;

                    await _httpClient.PutJsonAsync<Product>(_configuration["ApiHostUrl"] + $"api/v1.0/products/{product.Id}", product);
                }
                if (newOrder != null)
                {
                    await _httpClient.DeleteAsync(_configuration["ApiHostUrl"] + "api/v1.0/orders/" + newOrder.Id);
                }
                if (newOrderedProduct != null)
                {
                    await _httpClient.DeleteAsync(_configuration["ApiHostUrl"] + "api/v1.0/orderedproducts/" + newOrderedProduct.Id);
                }

                //Then we notify the customer with unsuccess message
                _toaster.Add($"Sorry something wrong, we can not send this order...", MatToastType.Danger, "Failed Sending:");
            }
        }
    }
}