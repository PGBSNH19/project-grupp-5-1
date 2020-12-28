using Frontend.Auth;
using Frontend.Services;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Frontend.Models;

namespace Frontend.Pages.LoginPages
{
    public class LoginBase: ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILocalStorageService localStorageService { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        [Inject]
        public IJSRuntime jSRuntime { get; set; }

        public User user = new User();
        public string LoginMesssage { get; set; }

        ClaimsPrincipal claimsPrincipal;

        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }

        protected async override Task OnInitializedAsync()
        {
            user = new User();

            claimsPrincipal = (await authenticationStateTask).User;

            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/index");
            }
        }

        public async Task<bool> ValidateUser()
        {
            //call an API
            var returnedUser = await userService.LoginAsync(user);

            if (returnedUser.Username != null)
            {
                await ((AuthStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(returnedUser);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                LoginMesssage = "Invalid username or password";
            }

            return await Task.FromResult(true);
        }
    }
}
