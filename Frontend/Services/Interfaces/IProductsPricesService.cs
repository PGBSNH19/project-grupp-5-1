using Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Services.Interfaces
{
    public interface IProductsPricesService
    {
        Task<IEnumerable<ProductPrice>> GetAllPrices();
        Task<ProductPrice> GetById(int id);
        Task<decimal> GetLatestPriceByProductId(int productId);
        Task<ProductPrice> GetPriceByProductId(int id);
        Task<ProductPrice> PostProductPrice(ProductPrice productPrice);        
        Task<ProductPrice> UpdatePrice(int id, ProductPrice productPriceDTO);
    }
}
