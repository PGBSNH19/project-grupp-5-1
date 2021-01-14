using Frontend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Frontend.Services
{
    public interface ICouponService
    {
        Task<IEnumerable<Coupon>> GetCoupons(bool getOnlyActive);

        Task<Coupon> GetCouponById(int id);

        Task<Coupon> CreateNewCoupon(Coupon coupon);

        Task UpdateCoupon(int id, Coupon coupon);
    }
}