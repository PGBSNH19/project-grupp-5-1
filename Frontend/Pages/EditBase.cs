using Frontend.Models;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{

    public class EditBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }
        public Product product { get; set; } = new Product();
        [Parameter]
        public string CurrentID { get; set; }

        public IEnumerable<Product> prod { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be over 0.")]
        public decimal ProductPrice { get; set; }

        public decimal SalePrice { get; set; }



        protected override async Task OnInitializedAsync()
        {
            product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));
            //prod = (await ProductService.GetProducts()).ToList();
            //var prices = (await ProductsPricesService.GetAllPrices()).ToList();
            //if (prices.Count != 0)
            //{
            //    foreach (var product in prod)
            //    {

            //        var price = prices.Where(x => x.ProductId == product.Id).FirstOrDefault();
            //        product.Price = price.Price;
            //        product.SalePrice = price.SalePrice;

            //    }

            //}
        }


    protected async void HandleValidSubmit()
        {
            await ProductService.Update(product, ProductPrice, SalePrice);
            product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));
            StateHasChanged();
            NavigationManager.NavigateTo("manageproducts");
        }
    }
}
