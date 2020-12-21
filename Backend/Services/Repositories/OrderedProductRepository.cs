﻿using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Repositories
{
    public class OrderedProductRepository : Repository<OrderedProduct>, IOrderedProductRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<OrderedProductRepository> _logger;

        public OrderedProductRepository(StoreDbContext context, ILogger<OrderedProductRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
