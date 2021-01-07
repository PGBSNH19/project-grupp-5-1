using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IProductsPricesRepository : IRepository<ProductPrice>
    {
        Task<Decimal> GetLatestPrice(int productId);
        Task<ProductPrice> GetPriceByProductId(int id);
    }
}