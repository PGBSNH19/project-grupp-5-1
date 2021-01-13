using Blazored.LocalStorage;
using Frontend.Models;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Frontend.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly NavigationManager _NavigationManager;
        private AuthenticationStateProvider _authenticationStateProvider;

        private readonly IMatToaster _toaster;
        private readonly IJSRuntime _jSRuntime;
        private readonly ILocalStorageService _localStorageService;

        public TokenValidator(NavigationManager NavigationManager, ILocalStorageService localStorageService, IJSRuntime jSRuntime, AuthenticationStateProvider authenticationStateProvider, IMatToaster toaster)
        {
            _jSRuntime = jSRuntime;
            _toaster = toaster;
            _NavigationManager = NavigationManager;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<HttpClient> CheckToken(HttpClient http)
        {
            var user = await _localStorageService.GetItemAsync<User>("user-details");

            if (user != null && DateTime.Now <= user.expiry)
            {
                string AccessToken = (await _localStorageService.GetItemAsync<User>("user-details")).AccessToken;
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                return http;
            }
            else
            {
                _toaster.Add($"You have to log in to go to this page.", MatToastType.Danger, "Alert:");
                await ((AuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                _NavigationManager.NavigateTo("/login");
                return http;
            }
        }
    }
}