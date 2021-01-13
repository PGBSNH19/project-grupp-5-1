using Blazored.Modal;
using Blazored.Modal.Services;
using Frontend.Models;
using Frontend.Pages;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frontend.Pages.Bases
{
    public class ManageCouponBase : ComponentBase
    {
        [Inject]
        public ICouponService CouponService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        public static int CreatedCouponId { get; set; }

        public Coupon CreateCoupon = new Coupon()
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Enabled = true
        };

        public Coupon GetCreatedCoupon;

        public IEnumerable<Coupon> Coupons { get; set; } = new List<Coupon>();
        public static int GetCoupnIdToUpodate { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Coupons = (await CouponService.GetCoupons(true)).Where(x => x.Enabled == true);
        }

        protected async Task HandleValidSubmit()
        {
            var result = await CouponService.CreateNewCoupon(CreateCoupon);

            if (result.Id != 0)
            {
                CreatedCouponId = result.Id;

                if (CreatedCouponId != 0)
                    GetCreatedCoupon = await CouponService.GetCouponById(CreatedCouponId);

                CreateCoupon = new Coupon()
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Enabled = true
                };

                Coupons = (await CouponService.GetCoupons(true)).Where(x => x.Enabled == true);
            }
        }

        public async Task ShowCouponPopup(int couponId, Coupon coupon)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(DeactivateCouponPopupBase.CouponId), couponId);
            parameters.Add(nameof(DeactivateCouponPopupBase.CouponToDeactivate), coupon);

            var shopModal = Modal.Show<DeactivateCouponPopup>("Deactivate Coupon", parameters);

            var result = await shopModal.Result;

            if (result.Cancelled) { Console.WriteLine("Modal was cancelled"); }
            else { StateHasChanged(); }
        }
    }
}