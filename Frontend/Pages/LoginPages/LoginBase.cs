﻿using Frontend.Auth;
using Frontend.Models;
using Frontend.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Pages.LoginPages
{
    public class LoginBase: ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IUserService userService { get; set; }

        public User user = new User();
        
        private User returnedUser;
        public string ErrorMesssage { get; set; }

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
            try
            {
                returnedUser = await userService.LoginAsync(user);
            }
            catch (System.Exception)
            {
                returnedUser = null;
            }

            if (returnedUser != null)
            {
                await ((AuthStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(returnedUser);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMesssage = "Invalid login info";
            }
            return await Task.FromResult(true);
        }
    }
}
