using Frontend.Auth;
using Frontend.Models;
using Frontend.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using Microsoft.JSInterop;

namespace Frontend.Pages.LoginPages
{
    public class SignupBase: ComponentBase
    {
        [Inject]
        public IJSRuntime _jSRuntime { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        public RegisterUser user;

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        ClaimsPrincipal loggedInUser;
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
                    await _jSRuntime.InvokeAsync<bool>("confirm", $"You have added the user successfully..");
                    user = new RegisterUser();
                    user.Role = "2";
                    StateHasChanged();
                }
                else
                {
                  await (((AuthStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(returnedUser));
                  NavigationManager.NavigateTo("/");
                }

            }
            else
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"This user is already registered..");
            }
            return await Task.FromResult(true);
        }

    }
}
