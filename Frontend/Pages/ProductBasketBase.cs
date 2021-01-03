using Blazored.LocalStorage;
using Blazored.Modal;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class ProductBasketBase : ComponentBase
    {
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; }

        public List<ProductInBasket> Basket = new List<ProductInBasket>();
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }

        private async Task GetBasketData()
        {
            bool basketExists = await LocalStorage.ContainKeyAsync("customer-basket");
            Basket = basketExists ? await LocalStorage.GetItemAsync<List<ProductInBasket>>("customer-basket") : new List<ProductInBasket>();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetBasketData();
            GetProductPrices = await ProductService.GetAllPrices();

            foreach (var item in Basket)
            {
                bool hasFound = GetProductPrices.Any(x => item.Product.Id == x.ProductId);
                if (hasFound)
                {
                    item.Product.Price = await ProductService.GetLatestPriceByProductId(item.Product.Id);
                    var saleprice = await ProductService.GetPriceByProductId(item.Product.Id);
                    item.Product.SalePrice = saleprice.SalePrice;
                }
                else
                {
                    item.Product.Price = 0;
                    item.Product.SalePrice = 0;
                }
            }
        }

        public async Task Cancel() => await BlazoredModal.Cancel();
    }
}