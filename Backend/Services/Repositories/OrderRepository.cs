using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(StoreDbContext context, ILogger<OrderRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}