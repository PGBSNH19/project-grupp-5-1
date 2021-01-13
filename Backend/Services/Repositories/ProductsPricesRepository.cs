using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Repositories
{
    public class ProductsPricesRepository : Repository<ProductPrice>, IProductsPricesRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ProductsPricesRepository> _logger;

        public ProductsPricesRepository(StoreDbContext context, ILogger<ProductsPricesRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Decimal> GetLatestPrice(int productId)
        {
            var query = await _context.Set<ProductPrice>().Where(x => x.ProductId == productId).OrderBy(x => x.DateChanged)
                .Select(x => x.SalePrice ?? x.Price).LastAsync();

            if (query == 0)
            {
                query = await _context.Set<ProductPrice>().Where(x => x.ProductId == productId).OrderBy(x => x.DateChanged)
                .Select(x => x.Price).LastAsync();
            }

            return query;
        }

        public async Task<ProductPrice> GetPriceByProductId(int id)
        {
            return await _context.Set<ProductPrice>().Where(x => x.ProductId == id).OrderBy(x => x.DateChanged)
                .Select(x => x).LastAsync();
        }
    }
}