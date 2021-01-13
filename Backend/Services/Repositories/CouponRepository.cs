using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly StoreDbContext _context;
        private ILogger<CouponRepository> _logger;

        public CouponRepository(StoreDbContext context, ILogger<CouponRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}