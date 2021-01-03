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
        Task<decimal> GetLatestPriceByProductId(int id);
        Task<ProductPrice> GetPriceByProductId(int id);
        Task<IEnumerable<ProductPrice>> GetAllPrices();
        Task<Product> AddProducts(Product product, decimal  productPrice, decimal salePrice);
        Task<Product> Update(Product product, decimal productPrice, decimal salePrice);
    }

}