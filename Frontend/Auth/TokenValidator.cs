using Frontend.Models;
using System.Net.Http;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using System;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly NavigationManager _NavigationManager;
        private AuthenticationStateProvider _authenticationStateProvider;

        private readonly IJSRuntime _jSRuntime;
        private readonly ILocalStorageService _localStorageService;


        public TokenValidator(NavigationManager NavigationManager, ILocalStorageService localStorageService, IJSRuntime jSRuntime, AuthenticationStateProvider authenticationStateProvider)
        {
            _jSRuntime = jSRuntime;
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
                await _jSRuntime.InvokeAsync<bool>("confirm", $"You have to log in to go to this page.");
                await ((AuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                _NavigationManager.NavigateTo("/login");
                return http;
            }
        }
    }
}
