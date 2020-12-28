using Frontend.Models;
using System.Net.Http;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace Frontend.Auth
{
    public class TokenValidator : ITokenValidator
    {
        private readonly NavigationManager _NavigationManager;

        private readonly IJSRuntime _jSRuntime;
        private readonly ILocalStorageService _localStorageService;

        public TokenValidator(NavigationManager NavigationManager, ILocalStorageService localStorageService, IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
            _NavigationManager = NavigationManager;
            _localStorageService = localStorageService;
        }

        public async Task<HttpClient> CheckToken(HttpClient http)
        {
            var user = await _localStorageService.GetItemAsync<User>("user-details");

            if (user != null)
            {
                string AccessToken = (await _localStorageService.GetItemAsync<User>("user-details")).AccessToken;
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                return http;
            }
            else
            {
                await _jSRuntime.InvokeAsync<bool>("confirm", $"You are not authorize to go to this page.");
                _NavigationManager.NavigateTo("/login");
                return null;
            }
        }
    }
}
