using Frontend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<OrderedProduct>> GetOrderedProducts();
        Task<OrderedProduct> GetOrderedProductById(int id);
    }
}