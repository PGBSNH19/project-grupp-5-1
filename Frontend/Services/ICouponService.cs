using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface ICouponService
    {
        Task<IEnumerable<Coupon>> GetCoupons(bool getOnlyActive);
        Task<Coupon> GetCouponById(int id);
        Task<Coupon> CreateNewCoupon(Coupon coupon);
    }
}
