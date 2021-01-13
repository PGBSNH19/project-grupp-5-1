using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend.Services.Repositories
{
    public class WeatherRepository : Repository<Weather>, IWeatherRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<WeatherRepository> _logger;

        public WeatherRepository(StoreDbContext context, ILogger<WeatherRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Weather> GetWeatherById(int weatherId)
        {
            return await _context.Set<Weather>().FirstOrDefaultAsync(w => w.Id == weatherId);
        }
    }
}