﻿using Frontend.Models;
using System.Collections.Generic;
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

        Task<Product> AddProducts(Product product, decimal productPrice);

        Task<Product> Update(Product product, decimal productPrice, decimal salePrice);

        Task<IEnumerable<Product>> SearchProducts(string productName);

        Task<IEnumerable<ProductCategory>> GetAllProductCategories();

        Task<IEnumerable<Product>> GetProductsByCategoryId(int id);

        Task DeleteProduct(int id);
    }
}