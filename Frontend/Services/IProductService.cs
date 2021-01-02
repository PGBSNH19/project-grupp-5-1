using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    
        Task<Product> GetProductById(int id);

        Task<Product> AddProducts(Product product, decimal  productPrice, decimal salePrice);
        Task<Product> Update(Product product, decimal productPrice, decimal salePrice);
        Task<IEnumerable<ProductPrice>> GetPrices();
    }

}