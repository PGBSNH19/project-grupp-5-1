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
        Task<IEnumerable<Product>> SearchProducts(string productName);
        Task<Product> GetProductById(int id);
        Task<Product> AddProducts(Product product);
        Task<Product> Update(Product product);
        Task<IEnumerable<ProductCategory>> GetAllProductCategories();
        Task<IEnumerable<Product>> GetProductsByCategoryId(int id);
        Task<IEnumerable<Product>> GetProductsByPriceRange(int min, int max);
    }

}