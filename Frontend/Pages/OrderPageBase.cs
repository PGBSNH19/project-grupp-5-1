using Frontend.Models;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Frontend.Services;
using System.Linq;
using System;
using MatBlazor;

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

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public ICouponService CouponService { get; set; }

        [Inject]
        protected IMatToaster Toaster { get; set; }

        [Inject]
        public IImageService ImageService { get; set; }

        [Parameter]
        public IEnumerable<ProductInBasket> basketproducts { get; set; } = null;

        public List<Image> images { get; set; }

        public IEnumerable<ProductPrice> GetProductPrices { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
        public string GetCouponId { get; set; } = "0";
        public decimal Discount { get; set; }
        public decimal TotalPriceWithDiscount { get; set; }

        [Parameter]
        public UserInfo userInfo { get; set; } = new UserInfo();

        protected override async Task OnInitializedAsync()
        {
            images = await ImageService.GetAllDefaultImages();
        }

        public async void Increase(ProductInBasket productInBasket)
        {
            if (productInBasket.Amount >= productInBasket.Product.Stock)
            {
                Toaster.Add($"You can't order more than {productInBasket.Product.Stock} of this product.", MatToastType.Danger, "Failed to add product");
            }
            else
            {
                Toaster.Add($"Added one \"{productInBasket.Product.Name}\" to your basket.", MatToastType.Info, "Added product");
                await OrderService.IncreaseProductToBasket(productInBasket.Product);
                productInBasket.Amount++;
                StateHasChanged();
            }
        }

        public async void Decrease(ProductInBasket productInBasket)
        {
            if (productInBasket.Amount <= 1)
            {
                Toaster.Add("You can't order less than one product.", MatToastType.Danger, "Error");
            }
            else
            {
                Toaster.Add($"Removed one \"{productInBasket.Product.Name}\" from your basket.", MatToastType.Info, "Removed product");
                productInBasket.Amount--;
                await OrderService.DecreaseProductToBasket(productInBasket.Product);
                StateHasChanged();
            }
        }

        public async void Remove(ProductInBasket productInBasket)
        {
            await OrderService.DeleteProductFromBasket(productInBasket);
            basketproducts = await OrderService.GetBasketProducts();
            Toaster.Add($"Removed \"{productInBasket.Product.Name}\" from your basket.", MatToastType.Success, "Removed product");
            StateHasChanged();
        }

        public async void SendOrder(UserInfo userInfo)
        {
            await OrderService.CreateOrder(userInfo, GetCouponId);
        }

        public void GetDiscount(string couponId)
        {
            Discount = Coupons.Where(x => x.Id == int.Parse(GetCouponId)).Select(d => d.Discount).FirstOrDefault();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                basketproducts = await OrderService.GetBasketProducts();
                userInfo.userBasket = basketproducts;

                GetProductPrices = await ProductService.GetAllPrices();
                foreach (var item in basketproducts)
                {
                    bool hasFound = GetProductPrices.Any(x => item.Product.Id == x.ProductId);
                    if (hasFound)
                    {
                        item.Product.CurrentPrice = await ProductService.GetLatestPriceByProductId(item.Product.Id);
                    }
                    else
                    {
                        item.Product.CurrentPrice = 0;
                    }
                }

                Coupons = (await CouponService.GetCoupons(true)).Where(x => x.Enabled == true);

                StateHasChanged();
            }
        }
    }
}