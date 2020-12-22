using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        //Task<Coupon> GetCouponById(int id);
        //Task<ICollection<Coupon>> GetAllCoupons();
    }
}
