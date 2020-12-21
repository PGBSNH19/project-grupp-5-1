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

        public IEnumerable<ProductInBasket> basketproducts { get; set; } = new List<ProductInBasket>();
        public IEnumerable<OrderedProduct> orderedProducts { get; set; } = new List<OrderedProduct>();
        public IEnumerable<int> userIds { get; set; } = new List<int>();

        [Inject]
        public IOrderService OrderService { get; set; }

       public async void Increase(Product product)
        {
            await OrderService.IncreaseProductToBasket(product);
            basketproducts = await OrderService.GetBasketProducts();

        }

        public async void Decrease(Product product)
        {
            await OrderService.DecreaseProductToBasket(product);
            basketproducts = await OrderService.GetBasketProducts();
        }

        public async void Remove(ProductInBasket product)
        {
            await OrderService.DeleteProductFromBasket(product);
            basketproducts = await OrderService.GetBasketProducts();
        }

        public async void SendOrder(IEnumerable<ProductInBasket> products)
        {
            await OrderService.CreateOrder(products);
            basketproducts = await OrderService.GetBasketProducts();
        }


        protected async override Task OnInitializedAsync()
        {
            basketproducts = await OrderService.GetBasketProducts();

        }
    }
}