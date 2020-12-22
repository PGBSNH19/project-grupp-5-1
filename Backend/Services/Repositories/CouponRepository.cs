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
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly StoreDbContext _context;
        private ILogger<CouponRepository> _logger;

        public CouponRepository(StoreDbContext context, ILogger<CouponRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        //public async Task<ICollection<Coupon>> GetAllCoupons()
        //{
        //    var query = _context.Coupon;
        //    return await query.ToListAsync();
        //}

        //public async Task<Coupon> GetCouponById(int id)
        //{
        //    return await _context.Set<Coupon>().FirstOrDefaultAsync(x => x.Id == id);
        //}       
    }
}
