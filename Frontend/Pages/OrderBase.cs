using System.Linq;
using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Frontend.Pages
{
    public class OrderBase : ComponentBase
    {

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IOrderService OrderService { get; set; }

        [Parameter]
        public IEnumerable<ProductInBasket> basketproducts { get; set; } = null;

       public async void Increase(Product product)
        {
            await OrderService.IncreaseProductToBasket(product);
        }

        public async void Decrease(Product product)
        {
            await OrderService.DecreaseProductToBasket(product);
        }

        public async void Remove(ProductInBasket product)
        {
            await OrderService.DeleteProductFromBasket(product);
        }

        public async void SendOrder(IEnumerable<ProductInBasket> products)
        {
            await OrderService.CreateOrder(products);
        }

        protected async override Task OnInitializedAsync()
        {
            System.Console.WriteLine("From the method");
            basketproducts = await OrderService.GetBasketProducts();
        }
    }
}