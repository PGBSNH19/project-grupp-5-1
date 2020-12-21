using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Bases
{
    public class ManageCouponBase  : ComponentBase
    {
        [Inject]
        public ICouponService CouponService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<Coupon> Coupons { get; set; } = new List<Coupon>();

        public Coupon Coupon = new Coupon()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Today
        };


        //protected async override Task OnInitializedAsync()
        //{
        //    Coupons = (await CouponService.GetCoupons(true)).Where(x => x.Enabled == true); 
        //}

        protected async Task HandleValidSubmit()
        {
            var result = await CouponService.CreateNewCoupon(Coupon);

            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
