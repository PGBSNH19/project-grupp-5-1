using Frontend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontend.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<ProductInBasket>> GetBasketProducts();
        Task DecreaseProductToBasket(Product product);
        Task IncreaseProductToBasket(Product product);
        Task DeleteProductFromBasket(ProductInBasket product);
        Task CreateOrder(IEnumerable<ProductInBasket> products);
    }
}