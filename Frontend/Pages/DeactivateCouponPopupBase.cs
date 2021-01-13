using System;
using System.Linq;
using Frontend.Models;
using Frontend.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.Modal.Services;
using Blazored.Modal;

namespace Frontend.Pages
{
    public class DeactivateCouponPopupBase : ComponentBase
    {
        [Inject]
        public ICouponService CouponService { get; set; }

        [CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }

        [Parameter]
        public int CouponId { get; set; }
        [Parameter]
        public Coupon CouponToDeactivate { get; set; }

        public bool CouponHasBeenDeactivated = false;

        public async Task Deactivate()
        {
            await UpdateCouponStatus();
            CouponHasBeenDeactivated = true;
        }
        public async Task UpdateCouponStatus()
        {
            CouponToDeactivate.Enabled = false;
            await CouponService.UpdateCoupon(CouponId, CouponToDeactivate);           
        }
        
        public async Task Cancel() => await ModalInstance.Cancel();
        public async Task Close() => await ModalInstance.Close(ModalResult.Ok($"Form was submitted successfully."));
    }
}
