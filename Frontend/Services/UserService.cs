﻿using System.Net.Http;
using Frontend.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Frontend.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<User> LoginAsync(User user)
        {
            var response = await _httpClient.PostJsonAsync<User>(_configuration["ApiHostUrl"] + "api/v1.0/login", user);
            return await Task.FromResult(response);
        }

        public async Task<User> RegisterUserAsync(RegisterUser user)
        {
            try
            {
                var response = await _httpClient.PostJsonAsync<User>(_configuration["ApiHostUrl"] + "api/v1.0/login/register", user);
                return await Task.FromResult(response);
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}