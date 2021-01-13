using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<ProductImageRepository> _logger;

        public ProductImageRepository(StoreDbContext context, ILogger<ProductImageRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}