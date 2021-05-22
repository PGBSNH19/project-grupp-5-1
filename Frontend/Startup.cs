using Frontend.Auth;
using Blazored.Modal;
using System.Net.Http;
using Frontend.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Frontend.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Frontend.Models;

namespace Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<WeatherForecastService>();

            services.AddScoped<ITokenValidator, TokenValidator>();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<IImageService, ImageService>();
            services.AddHttpClient<IOrderService, OrderService>();
            services.AddHttpClient<ICouponService, CouponService>();
            services.AddHttpClient<IProductService, ProductService>();

            services.AddBlazoredModal();
            services.AddBlazoredLocalStorage();

            services.AddOptions();
            services.Configure<AzureStorageConfig>(Configuration.GetSection("AzureStorageConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}