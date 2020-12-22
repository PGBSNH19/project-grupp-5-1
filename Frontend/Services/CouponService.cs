using Frontend.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class CouponService : ICouponService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;

        public CouponService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this._configuration = configuration;
        }

        public async Task<Coupon> CreateNewCoupon(Coupon coupon)
        {
            return await httpClient.PostJsonAsync<Coupon>(_configuration["ApiHostUrl"] + "api/v1.0/coupons", coupon);
        }

        public async Task<Coupon> GetCouponById(int id)
        {
            return await httpClient.GetJsonAsync<Coupon>(_configuration["ApiHostUrl"] + $"api/v1.0/coupons/{id}");
        }

        public async Task<IEnumerable<Coupon>> GetCoupons(bool getOnlyActive)
        {
            return await httpClient.GetJsonAsync<List<Coupon>>(_configuration["ApiHostUrl"] + $"api/v1.0/coupons/");
        }
    }
}