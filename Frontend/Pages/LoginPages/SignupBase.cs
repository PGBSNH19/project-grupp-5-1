using Frontend.Auth;
using Frontend.Models;
using Frontend.Services;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frontend.Pages.LoginPages
{
    public class SignupBase : ComponentBase
    {
        [Inject]
        public IJSRuntime _jSRuntime { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IMatToaster Toaster { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        public RegisterUser user;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private ClaimsPrincipal loggedInUser;
        public bool IsUserAuthenticated;
        public bool IsUserAdmin;

        protected async override Task OnInitializedAsync()
        {
            loggedInUser = (await authenticationStateTask).User;

            if (loggedInUser.Identity.IsAuthenticated)
            {
                IsUserAuthenticated = true;
            }
            if (loggedInUser.IsInRole("Admin"))
            {
                IsUserAdmin = true;
            }

            user = new RegisterUser();
            user.Role = "2";
        }

        public async Task<bool> RegisterUser()
        {
            var returnedUser = await userService.RegisterUserAsync(user);

            if (returnedUser != null)
            {
                if (IsUserAdmin)
                {
                    Toaster.Add($"User \"{returnedUser.Username}\" has successfully been registered.", MatToastType.Success, "Account registered");
                    user = new RegisterUser();
                    user.Role = "2";
                    StateHasChanged();
                }
                else
                {
                    Toaster.Add("Your account has successfully been registered!", MatToastType.Success, "Account registered");
                    await (((AuthStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(returnedUser));
                    NavigationManager.NavigateTo("/");
                }
            }
            else
            {
                Toaster.Add($"An account with the username \"{user.Username}\" already exists.", MatToastType.Danger, "Username already exists");
            }
            return await Task.FromResult(true);
        }
    }
}