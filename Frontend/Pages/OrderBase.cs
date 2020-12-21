using Frontend.Models;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class OrderBase : ComponentBase
    {
        public IEnumerable<Order> orders { get; set; } = new List<Order>();
        public IEnumerable<OrderedProduct> orderedProducts { get; set; } = new List<OrderedProduct>();
        public IEnumerable<int> userIds { get; set; } = new List<int>();

        [Inject]
        public IOrderService ProductService { get; set; }


        protected async override Task OnInitializedAsync()
        {
            orders = (await ProductService.GetOrders()).OrderBy(u => u.UserId);
            orderedProducts = await ProductService.GetOrderedProducts();

            foreach (var order in orders)
            {
                var thisOrderedProducts = orderedProducts.Where(o => o.OrderId == order.Id);
                order.OrderedProducts = thisOrderedProducts;
            }

        }
    }
}