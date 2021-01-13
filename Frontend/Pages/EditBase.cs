using Frontend.Componenets;
using Frontend.Models;
using Frontend.Services;
using Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Pages
{
    public class EditBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IImageService ImageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public Product product { get; set; } = new Product();

        [Parameter]
        public string CurrentID { get; set; }

        public string ProductCatId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public IEnumerable<Product> products { get; set; }
        public IEnumerable<ProductPrice> GetProductPrices { get; set; }

        public DropZoneBase child;

        protected override async Task OnInitializedAsync()
        {
            product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));
            ProductCategories = (await ProductService.GetAllProductCategories()).ToList();

            GetProductPrices = await ProductService.GetAllPrices();
            product.ProductCategoryName = ProductCategories.Where(p => p.Id == product.ProductCategoryId).SingleOrDefault()?.CategoryName;

            bool hasFound = GetProductPrices.Any(x => product.Id == x.ProductId);
            if (hasFound)
            {
                var getProductPrices = await ProductService.GetPriceByProductId(product.Id);
                product.Price = getProductPrices.Price;
                product.SalePrice = getProductPrices.SalePrice;
                product.CurrentPrice = await ProductService.GetLatestPriceByProductId(product.Id);
            }
            else
            {
                product.CurrentPrice = 0;
                product.Price = 0;
                product.SalePrice = 0;
            }
        }

        protected async void HandleValidSubmit()
        {
            if (await ImageService.UploadImages(child.Images, product.Id))
            {
                if (ProductCatId == null)
                    ProductCatId = "1";

                product.ProductCategoryId = int.Parse(ProductCatId);
                await ProductService.Update(product, product.Price, (decimal)product.SalePrice);
                product = await Task.Run(() => ProductService.GetProductById(Convert.ToInt32(CurrentID)));

                StateHasChanged();
                NavigationManager.NavigateTo("manageproducts");
            }
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("manageproducts");
        }
    }
}