using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(StoreDbContext context, ILogger<ProductRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Set<Product>().FirstOrDefaultAsync(w => w.Id == id);
        }
        //public async Task<Decimal> GetLatestPrice(int productId)
        //{
        //    return await _context.Set<ProductPrice>().Where(x => x.ProductId == productId).OrderBy(x => x.DateChanged)
        //        .Select(x => x.SalePrice ?? x.Price).LastAsync();
        //}
    }
}