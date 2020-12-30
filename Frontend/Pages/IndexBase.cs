using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<Product> products { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public string ProductSearchQuery { get; set; }
        public string ProductCategoryId { get; set; }


        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProducts()).ToList();
            ProductCategories = await ProductService.GetAllProductCategories();
        }

        protected async Task SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(ProductSearchQuery))
                products = (await ProductService.GetProducts()).ToList();
            else
                products = (await ProductService.SearchProducts(ProductSearchQuery)).ToList();
        }

        protected async Task FilterByProductCategory()
        {
            var id = int.Parse(ProductCategoryId);

            if (id == 0)
                products = await ProductService.GetProducts();
            else
                products = await ProductService.GetProductsByCategoryId(id);
        }

    }
}