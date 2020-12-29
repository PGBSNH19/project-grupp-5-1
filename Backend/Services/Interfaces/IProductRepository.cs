﻿using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductById(int id);
        Task<IList<Product>> SearchProducts(string productName);
    }
}