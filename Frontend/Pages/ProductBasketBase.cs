using Blazored.LocalStorage;
using Blazored.Modal;
using Frontend.Models;
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
        public ILocalStorageService localStorage { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; }

        public List<ProductInBasket> Basket = new List<ProductInBasket>();

        private async Task GetBasketData()
        {
            bool basketExists = await localStorage.ContainKeyAsync("customer-basket");
            Basket = basketExists ? await localStorage.GetItemAsync<List<ProductInBasket>>("customer-basket") : new List<ProductInBasket>();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetBasketData();
        }

        public async Task Cancel() => await BlazoredModal.Cancel();
    }
}