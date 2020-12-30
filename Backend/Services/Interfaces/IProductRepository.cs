using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductById(int id);
        Task<IList<Product>> SearchProducts(string productName);
        Task<IList<Product>> GetProductsByCategory(string category);
        Task<IList<Product>> GetProductsByPriceRange(int min, int max);
    }
}