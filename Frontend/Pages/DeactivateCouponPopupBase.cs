using Blazored.Modal;
using Blazored.Modal.Services;
using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class DeactivateCouponPopupBase : ComponentBase
    {
        [Inject]
        public ICouponService CouponService { get; set; }

        [CascadingParameter] private BlazoredModalInstance ModalInstance { get; set; }

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