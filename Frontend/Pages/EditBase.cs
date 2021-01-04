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
        //public Product product { get; set; } = new Product();
        [Parameter]
        //public string CurrentID { get; set; }

        public IEnumerable<Product> products { get; set; }
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be over 0.")]
        public decimal ProductPrice { get; set; }

        public decimal? SalePrice { get; set; }



        //protected override async Task OnInitializedAsync()
        //{
        //    product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));
        //    //GetProductPrices = await ProductService.GetAllPrices();

        //    //foreach (var product in products)
        //    //{
        //    //    bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
        //    //    if (hasFound)
        //    //        product.Price = await ProductService.GetLatestPriceByProductId(product.Id);
        //    //    else
        //    //        product.Price = 0;
        //    //}
        //}


        //protected async void HandleValidSubmit()
        //{
        //    product.ProductCategoryId = int.Parse(ProductCatId);
        //    await ProductService.Update(product, ProductPrice, (decimal)SalePrice);
        //    product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));
        //    StateHasChanged();
        //    NavigationManager.NavigateTo("manageproducts");
        //}
    }
}
