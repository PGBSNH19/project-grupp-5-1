using Frontend.Models;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public AuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var storedUser = await _localStorageService.GetItemAsync<User>("user-details");

            ClaimsIdentity identity;

            if (storedUser != null)
            {
                identity = GetClaimsIdentity(storedUser);
            }
            else
            {
                identity = new ClaimsIdentity();
            }          

            var claimsPrincipal = new ClaimsPrincipal(identity);            

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(User user)
        {
            await _localStorageService.SetItemAsync("user-details", user);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("user-details");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Username != null)
            { 
                claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, (user.FirstName + " " + user.LastName) ?? user.Username),                                   
                    new Claim(ClaimTypes.UserData, user.Username),                                   
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }, "apiauth_type");
            }

            return claimsIdentity;
        }
    }
}
