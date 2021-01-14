using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Backend.Services.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(StoreDbContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}