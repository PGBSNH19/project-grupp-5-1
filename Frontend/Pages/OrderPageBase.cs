﻿using Frontend.Models;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages
{
    public class OrderPageBase : ComponentBase
    {
        [Inject]
        public IJSRuntime _jSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IOrderService OrderService { get; set; }

        [Parameter]
        public IEnumerable<ProductInBasket> basketproducts { get; set; } = null;

        [Parameter]
        public UserInfo userInfo { get; set; } = new UserInfo();

        public async void Increase(ProductInBasket product)
        {
            if (product.Amount >= product.Product.Stock) await _jSRuntime.InvokeAsync<bool>("confirm", $"You can't order more than {product.Product.Stock} of this product.");
            else
            {
                await OrderService.IncreaseProductToBasket(product.Product);
                product.Amount++;
                StateHasChanged();
            }
        }

        public async void Decrease(ProductInBasket productInBasket)
        {
            if (productInBasket.Amount <= 1) await _jSRuntime.InvokeAsync<bool>("confirm", $"You can't order less than one product.");
            else
            {
                productInBasket.Amount--;
                await OrderService.DecreaseProductToBasket(productInBasket.Product);
                StateHasChanged();
            }
        }

        public async void Remove(ProductInBasket product)
        {
            await OrderService.DeleteProductFromBasket(product);
            basketproducts = await OrderService.GetBasketProducts();
            StateHasChanged();
        }

        public async void SendOrder(UserInfo userInfo)
        {
            await OrderService.CreateOrder(userInfo);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                basketproducts = await OrderService.GetBasketProducts();
                userInfo.userBasket = basketproducts;
                StateHasChanged();
            }
        }
    }
}